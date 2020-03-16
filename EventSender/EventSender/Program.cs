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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSender
{
    class Program
    {
        private static NewsApiClient newsApiClient;

        private static async Task<IEnumerable<ArticleModel>> GetNews(string lastNews, string keyword)
        {
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = keyword,
                SortBy = SortBys.PublishedAt,
                Language = Languages.EN,
                From = DateTime.Now.Date,
                PageSize = 100
            });

            return articlesResponse.Articles.TakeWhile(a => a.Title != lastNews).Map();
        }

        static async Task Main()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();


            var connectionString = config.GetConnectionString("EventHub");
            var eventHubName = config["EventHubName"];
            var keyWord = config["Keyword"];
            var newsApiKey = config["NewsApiKey"];
            var delay = Convert.ToInt32(config["Delay"]) * 1000;
            newsApiClient = new NewsApiClient(newsApiKey);
            var lastNews = "";

            await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
            {
                while (true)
                {
                    var eventBatch = await producerClient.CreateBatchAsync();
                    var newNews = (await GetNews(lastNews, keyWord)).ToList();

                    if (newNews.Any())
                    {
                        lastNews = newNews.First().Title;
                        newNews.ForEach(news =>
                        {
                            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(news))));
                        });
                        await producerClient.SendAsync(eventBatch);
                    }

                    Console.WriteLine($"A batch of {newNews.Count} events has been published.");
                    await Task.Delay(delay);
                }
            }
        }
    }
}
