using BSM.DataServer.websocket;
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

        private IDataProcessor dataProcessor;

        public BSMDataHandler()
        {
            dataProcessor = new BSMDataProcessor();
        }

        public Datagram ParseDatagram(byte[] bytes)
        {
            Datagram datagram = new Datagram();

            datagram.Bytes = bytes;

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
                    if (datagram.Type == Datagram.TYPE_DATA ||
                        datagram.Type == Datagram.TYPE_ADDR_SET_COMMIT || 
                        datagram.Type == Datagram.TYPE_HEART_BEAT ||
                        datagram.Type == Datagram.TYPE_REGISTER || 
                        datagram.Type == Datagram.TYPE_TILT_RESET_COMMIT)
                    {
                        datagram.Data = new byte[datagram.Length - 1];
                        Array.Copy(bytes, 5, datagram.Data, 0, datagram.Length - 1);
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

            return datagram;
        }

        public async Task Receive(IPEndPoint remoteIp, byte[] bytes)
        {
            try
            {
                var datagram = ParseDatagram(bytes);
                datagram.From = remoteIp;
                
                Log.Debug("Datagram received from {0}: {1}", remoteIp.ToString(), datagram.ToString());

                WebSocketManager.Instance.BroadcastDatagram(datagram);

                await this.dataProcessor.Process(datagram);
            } catch(Exception ex)
            {
                Log.Error(ex, "Error when receive datagram");
            }
            
        }
    }
}
