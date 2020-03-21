using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using EventSender.Models;
using EventSender.Models.Mappings;
using Microsoft.Extensions.Configuration;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventSender
{
    class Program
    {
        public class LatestResponse
        {
            public string Title { get; set; }
            public string Keyword { get; set; }
        }

        private static NewsApiClient newsApiClient;

        private static async Task<IEnumerable<ArticleModel>> GetNews(Dictionary<string, string> state)
        {
            var result = new List<ArticleModel>();
            var newState = new Dictionary<string, string>();
            foreach (var keyword in state.Keys)
            {
                var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
                {
                    Q = keyword,
                    SortBy = SortBys.PublishedAt,
                    Language = Languages.EN,
                    From = DateTime.Now.Date,
                    PageSize = 100
                });

                result.AddRange(articlesResponse
                                .Articles
                                .TakeWhile(a => a.Title != state[keyword])
                                .Map(keyword)
                                .ToList());

                if (articlesResponse.Articles.Any())
                {
                    newState[keyword] = articlesResponse.Articles.First().Title;
                }
            }

            foreach(var key in newState.Keys)
            {
                state[key] = newState[key];
            }

            return result;
        }

        private static async Task<Dictionary<string, string>> GetInitialState(IConfiguration config)
        {
            var keywords = config.GetSection("Keywords")
                     .GetChildren()
                     .Select(x => x.Value)
                     .ToDictionary(k => k, v => string.Empty);

            HttpClient client = new HttpClient();

            var response = await (await client.GetAsync(config["GetLatestUrl"])).Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<LatestResponse>>(response);

            foreach (var entry in content)
            {
                if (keywords.ContainsKey(entry.Keyword))
                {
                    keywords[entry.Keyword] = entry.Title;
                }
            }

            return keywords;
        }

        static async Task Main()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();


            var connectionString = config.GetConnectionString("EventHub");
            var eventHubName = config["EventHubName"];
            var state = await GetInitialState(config);
            var newsApiKey = config["NewsApiKey"];
            var delay = Convert.ToInt32(config["Delay"]) * 1000;
            newsApiClient = new NewsApiClient(newsApiKey);

            await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
            {
                var news = new List<ArticleModel>();
                var rng = new Random();

                while (true)
                {
                    var eventBatch = await producerClient.CreateBatchAsync();
                    var newNews = await GetNews(state);
                    if (newNews.Any())
                    {
                        news.AddRange(newNews);
                    }

                    if (news.Any())
                    {
                        var count = rng.Next(1, Math.Min(5, news.Count));
                        var toBeSent = news.Take(count).ToList();
                        toBeSent.ForEach(news =>
                        {
                            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(news))));
                        });
                        await producerClient.SendAsync(eventBatch);
                        Console.WriteLine($"A batch of {toBeSent.Count} events has been published.");

                        news = news.Skip(count).ToList();
                    }

                    await Task.Delay(delay);
                }
            }
        }
    }
}
