using System;

namespace BSM.DataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProviders.ConfigServiceProvider();
            Logging.ConfigureLogger();
            Config.LoadAppSettings();            

            UdpServer udpserver = new UdpServer();
            udpserver.init();
            
            Console.ReadLine();
        }
    }
}
