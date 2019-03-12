using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace BSM.DataServer
{
    public class Config
    {
        public static int UdpPort
        {
            get; private set;
        }

        public static string UdpHost
        {
            get; private set;
        }

        public static bool UdpEnabled
        {
            get; private set;
        }

        public static String DBConnection
        {
            get; private set;
        }

        public static String WebHost
        {
            get; private set;
        }

        public static int WebPort
        {
            get; private set;
        }

        
        public static void LoadAppSettings()
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

                DBConnection = config.GetConnectionString("BSMDatabase");
                WebHost = config["web:host"];
                WebPort = int.Parse(config["web:port"]);
                UdpHost = config["udp:host"];
                UdpPort = int.Parse(config["udp:port"]);
                UdpEnabled = bool.Parse(config["udp:enabled"]);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error when load settings from appsettings.json");
            }
        }
    }
}
