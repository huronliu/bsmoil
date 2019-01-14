using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BSM.DataServer
{
    class BSMDataHandler : IDatagramHandler
    {
        public static readonly byte HEADER_1 = 0x68;
        public static readonly byte HEADER_2 = 0x86;
        public static readonly byte TAIL_1 = 0x16;

        public Datagram ParseDatagram(byte[] bytes)
        {
            Datagram datagram = new Datagram();

            if (bytes == null || bytes.Length < 5)
            {
                datagram.State = DatagramState.InvalidLength;
            }

            if (bytes[0] == HEADER_1 && bytes[1] == HEADER_2)
            {
                //Get data length
                datagram.Length = bytes[2] * 0x100 + bytes[3];

                if (bytes.Length >= datagram.Length + 5)
                {
                    //Get data type
                    datagram.Type = bytes[4];
                    if (datagram.Type == 1)
                    {
                        datagram.Data = new byte[datagram.Length - 2];
                        Array.Copy(bytes, 5, datagram.Data, 0, datagram.Length - 2);
                    } else
                    {
                        datagram.State = DatagramState.InvalidType;
                    }
                } else
                {
                    datagram.State = DatagramState.InvalidLength;
                }
            } else
            {
                datagram.State = DatagramState.InvalidHeader;
            }

            if (datagram.State != DatagramState.OK)
            {
                Log.Error("Datagram with {0}: {1}", datagram.State.ToString("G"), BitConverter.ToString(bytes));
            }
            
            return datagram;
        }

        public void Receive(IPEndPoint remoteIp, byte[] bytes)
        {
            var datagram = ParseDatagram(bytes);
            Log.Debug("FROM {0}: {1}", remoteIp.ToString(), datagram.ToString());
        }
    }
}
