using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BSM.DataServer
{
    interface IDatagramHandler
    {
        void Receive(IPEndPoint remoteIp, byte[] bytes);
        Datagram ParseDatagram(byte[] bytes);
    }
}
