using BSM.DataServer.websocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace BSM.DataServer
{
    public class Startup
    {

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            WebSocketManager wsmanager = WebSocketManager.Instance;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var websocketOptions = new WebSocketOptions()
            {
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(websocketOptions);
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                        await wsmanager.NewClientConnected(context, ws);
                    }
                    else  
                    {
                        context.Response.StatusCode = 400;
                    }
                } else
                {
                    await next();
                }
            });               
        }
    }
}
