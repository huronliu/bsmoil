using System.Net;
using System.Threading.Tasks;

namespace BSM.DataServer
{
    interface IDatagramHandler
    {
        Task Receive(IPEndPoint remoteIp, byte[] bytes);
        Datagram ParseDatagram(byte[] bytes);
    }
}
