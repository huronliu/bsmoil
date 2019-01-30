using System.Net;

namespace BSM.DataServer
{
    interface IDatagramHandler
    {
        void Receive(IPEndPoint remoteIp, byte[] bytes);
        Datagram ParseDatagram(byte[] bytes);
    }
}
