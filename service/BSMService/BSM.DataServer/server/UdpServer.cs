using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using Serilog;

namespace BSM.DataServer
{
    public class UdpServer
    {
        private UdpClient udpListener;
        private IDatagramHandler handler;

        public UdpServer()
        {
            handler = new BSMDataHandler();
            Inited = false;
        }

        public bool Inited
        {
            get; private set;
        }

        public bool Running
        {
            get; private set;
        }

        public void Stop()
        {
            this.Running = false;
        }

        public void Init()
        {            
            try
            {
                Inited = false;

                IPEndPoint groupep = new IPEndPoint(
                Config.UdpHost.Equals("any", StringComparison.OrdinalIgnoreCase) ?
                    IPAddress.Any : IPAddress.Parse(Config.UdpHost),
                    Config.UdpPort);
                udpListener = new UdpClient(groupep);

                Log.Information("Udp Server is initialized and listening on: {0}:{1}", Config.UdpHost, Config.UdpPort);
                Inited = true;
            } 
            catch(SocketException se)
            {
                Log.Error(se, "Error when initialize the Udp Listener");
            }
            
            if (Inited == true)
            {
                Task.Factory.StartNew(async () =>
                {
                    Running = true;
                    while (Running)
                    {
                        var result = await udpListener.ReceiveAsync();
                        if (result != null)
                        {
                            handler.Receive(result.RemoteEndPoint, result.Buffer).Start();                            
                        }
                    }
                });
            }
            
        }
    }
}
