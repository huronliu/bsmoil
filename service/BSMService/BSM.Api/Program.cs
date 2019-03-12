using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BSM.Common;
using BSM.Common.DB;
using BSM.DataServer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BSM.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //Configure the Logger
                Logging.ConfigureLogger();

                //Load the configuration from appsettings.json
                Config.LoadAppSettings();

                //Initialize DB
                using (BSMContext context = new BSMContext(Config.DBConnection))
                {
                    if (context.Database.EnsureCreated())
                    {
                        Log.Information("Database schemas initialized");
                    }
                    Log.Information("Database initialized: {0}", context.DataSourceString);
                }

                //start up the udp server
                if (Config.UdpEnabled)
                {
                    UdpServer udpserver = new UdpServer();
                    udpserver.Init();
                }
                else
                {
                    Log.Information("UDP server is disabled, will not start up to listen on udp port");
                }

                //Start the web server host (include websocket)
                CreateWebHostBuilder(args).Build().Run();                
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when startup the service");
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls($"http://{Config.WebHost}:{Config.WebPort}/")
                .UseStartup<Startup>();
            
    }
}
