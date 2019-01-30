using System;
using BSM.Common;
using BSM.Common.DB;
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
            
            Console.ReadLine();
            udpserver.Stop();
        }
    }
}
