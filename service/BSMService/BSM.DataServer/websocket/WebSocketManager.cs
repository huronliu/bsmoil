using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Threading;
using Newtonsoft.Json;

namespace BSM.DataServer.websocket
{
    public class WebSocketManager
    {
        public static WebSocketManager Instance { get; } = new WebSocketManager();

        private IDictionary<String, WebSocket> clients = new Dictionary<String, WebSocket>();

        public async Task NewClientConnected(HttpContext context, WebSocket websocket)
        {
            try
            {
                string remoteip = $"{context.Connection.RemoteIpAddress.MapToIPv4().ToString()}:{context.Connection.RemotePort}";
                Log.Information("WS client connected- {0}", remoteip);

                clients.Add(remoteip, websocket);

                var buffer = new byte[1024 * 4];                
                WebSocketReceiveResult result = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {
                    Log.Information("Received: {0}", UTF8Encoding.UTF8.GetString(buffer, 0, result.Count));
                    result = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                await websocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                clients.Remove(remoteip);
                Log.Information("WS client disconnected: {0} - {1}", result.CloseStatus.Value.ToString("G"), remoteip);
            } catch(Exception ex)
            {
                Log.Error(ex, "Error occurred while websocket client connecting");
            }
            
        }

        public void BroadcastDatagram(Datagram datagram)
        {
            Task.Factory.StartNew(async () =>
            {
                if (clients.Count > 0)
                {
                    foreach (var c in clients.Values)
                    {
                        try
                        {
                            if (c.State == WebSocketState.Open)
                            {
                                byte[] buffer;
                                string json = JsonConvert.SerializeObject(new
                                {
                                    type = "datagram", 
                                    data = new
                                    {
                                        type = datagram.Type,
                                        state = datagram.State.ToString("G"),
                                        from = $"{datagram.From.Address.ToString()}:{datagram.From.Port}",
                                        bytes = BitConverter.ToString(datagram.Bytes),
                                        time = DateTime.Now.ToString("HH:mm:ss.ffff")
                                    }                                    
                                });
                                buffer = UTF8Encoding.UTF8.GetBytes(json);
                                await c.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Error when send datagram to websocket client: {ex.Message}");
                        }
                    }
                }
            });            
        }
    }
}
