using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using EventHubsSender.Models;
using EventHubsSender.Models.Mappings;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubsSender
{
    class Program
    {
        private const string connectionString = "Endpoint=sb://eventhubsnamespace1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=wA0tsDOfbeWVZjOz3jryUDM/qJwcKs1LpJO5shhBCYs=";
        private const string eventHubName = "EventHub1";
        private static NewsApiClient newsApiClient = new NewsApiClient("20980be009cb41ba94becb4c008f47c2");



        private static async Task<IEnumerable<ArticleModel>> GetNews(string lastNews)
        {
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = "Corona",
                SortBy = SortBys.PublishedAt,
                Language = Languages.EN,
                From = DateTime.Now.Date,
                PageSize = 100
            });

            return articlesResponse.Articles.TakeWhile(a => a.Title != lastNews).Map();
        }

        static async Task Main()
        {
            var lastNews = "";

            await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
            {
                while (true)
                {
                    var eventBatch = await producerClient.CreateBatchAsync();
                    var newNews = (await GetNews(lastNews)).ToList();

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
                    await Task.Delay(10000);
                }

            }
        }
    }
}
