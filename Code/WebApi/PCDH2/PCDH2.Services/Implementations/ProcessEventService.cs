using AutoMapper;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using PCDH2.Core.Entities;
using PCDH2.Services.Contracts;
using PCDH2.Services.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PCDH2.Services.Implementations
{
    public static class ProcessEventService
    {
        private static IServiceProvider serviceProvider;

        public static event EventHandler<GenericEventArg> ArticlePosted;
        public static Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            var mapper = (IMapper)serviceProvider.GetService(typeof(IMapper));
            var articleService = (IArticleService)serviceProvider.GetService(typeof(IArticleService));
            var json = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
            var model = JsonConvert.DeserializeObject<ArticleModel>(json);
            var article = mapper.Map<Article>(model);

            articleService.AddArticle(article);

            ArticlePosted?.Invoke(null, new GenericEventArg { Data = article });

            return Task.CompletedTask;
        }

        public static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }

        public static void Configure(IApplicationBuilder app)
        {
            serviceProvider = app.ApplicationServices;
        }
    }
}
