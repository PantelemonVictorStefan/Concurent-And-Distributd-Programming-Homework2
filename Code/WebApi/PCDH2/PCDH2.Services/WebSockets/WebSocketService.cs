using Microsoft.AspNetCore.Http;
using PCDH2.Services.WebSockets;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCDH2.Services.Implementations
{
    public static class WebSocketService
    {
        public static async Task HandleConnection(HttpContext context, WebSocket webSocket)
        {

            var connection = new WebSocketConnectionHandler(context, webSocket);

            connection.Subscribe();

            var buffer = new byte[1024 * 4];

            WebSocketReceiveResult result;
            do
            {

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            while (!result.CloseStatus.HasValue);
            connection.Unsubscribe();
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);


        }
    }
}
