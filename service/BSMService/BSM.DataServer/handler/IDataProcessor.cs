
using System.Threading.Tasks;

namespace BSM.DataServer
{
    interface IDataProcessor
    {
        Task Process(Datagram datagram);
    }
}
