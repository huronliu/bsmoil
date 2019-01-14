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

        public static String DBConnection
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

                UdpHost = config["udpServer:host"]; 
                UdpPort = int.Parse(config["udpServer:port"]);
                DBConnection = config.GetConnectionString("BSMDB");
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error when load settings from appsettings.json");
            }
        }
    }
}
