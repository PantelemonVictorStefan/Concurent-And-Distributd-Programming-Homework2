using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PCDH2.Services.Implementations;
using PCDH2.Services.Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCDH2.Services.WebSockets
{
    public class WebSocketConnectionHandler
    {
        protected HttpContext context;

        protected WebSocket webSocket;

        public WebSocketConnectionHandler(HttpContext context, WebSocket webSocket)
        {
            this.context = context;
            this.webSocket = webSocket;
        }

        public void Subscribe()
        {
            ProcessEventService.ArticlePosted += NotifyUser;
        }

        public void Unsubscribe()
        {
            ProcessEventService.ArticlePosted -= NotifyUser;
        }
        private void NotifyUser(object sender, GenericEventArg e)
        {
            var json = JsonConvert.SerializeObject(e.Data);
            var msg = Encoding.ASCII.GetBytes(json);

            webSocket.SendAsync(msg, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
