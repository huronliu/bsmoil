using System;
using BSM.Common;
using BSM.Common.DB;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace BSM.DataServer
{
    class Program
    {
        static void Main(string[] args)
        {            
            Logging.ConfigureLogger();
            Config.LoadAppSettings();            
            using(BSMContext context = new BSMContext(Config.DBConnection))
            {
                if (context.Database.EnsureCreated())
                {
                    Log.Information("Database schemas initialized");
                }
                Log.Information("Database initialized: {0}", context.DataSourceString);
            }

            UdpServer udpserver = new UdpServer();
            udpserver.Init();

            var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseUrls($"http://{Config.WebSocketHost}:{Config.WebSocketPort}/")
                        .UseStartup<Startup>()
                        .Build();
            host.Start();
            Log.Information("WebSocket server is started to listen on: ws://{0}:{1}/ws", Config.WebSocketHost, Config.WebSocketPort);

            Console.ReadLine();
            udpserver.Stop();
        }
    }
}
