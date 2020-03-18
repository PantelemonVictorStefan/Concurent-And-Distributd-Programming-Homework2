using AutoMapper;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Storage.Blobs;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PCDH2.API;
using PCDH2.Core.Contracts.Repositories;
using PCDH2.Services.Contracts;
using PCDH2.Services.Implementations;
using System;
using System.Net.WebSockets;

namespace Presentation.PCDH2.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();

            services.AddControllers();

            services.AddAutoMapper(c => c.AddProfile<AutomapperProfile>(), typeof(Startup));

            AddDependencies(services);
        }

        public void AddDependencies(IServiceCollection services)
        {
            services.AddTransient<IArticleService, ArticleService>();

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddDbContext<FeedContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureWebSockets(app);

            ConfigureEventReceiver(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureWebSockets(IApplicationBuilder app)
        {
            app.UseWebSockets();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            //webSocketOptions.AllowedOrigins.Add("https://client.com");

            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await WebSocketService.HandleConnection(context, webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });
        }

        private void ConfigureEventReceiver(IApplicationBuilder app)
        {
            ProcessEventService.Configure(app);

            string blobStorageConnectionString = Configuration["blobStorageConnectionString"];
            string blobContainerName = Configuration["blobContainerName"];
            string ehubNamespaceConnectionString = Configuration["ehubNamespaceConnectionString"];
            string eventHubName = Configuration["eventHubName"];

            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            // Create a blob container client that the event processor will use 
            BlobContainerClient storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);

            // Create an event processor client to process events in the event hub
            EventProcessorClient processor = new EventProcessorClient(storageClient, consumerGroup, ehubNamespaceConnectionString, eventHubName);

            // Register handlers for processing events and handling errors
            processor.ProcessEventAsync += ProcessEventService.ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessEventService.ProcessErrorHandler;

            // Start the processing
            processor.StartProcessingAsync();
        }
    }
}
