using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BSM.DataServer
{
    public class Datagram
    {
        public static readonly byte TYPE_DATA = 0x01;
        public static readonly byte TYPE_ADDR_SET = 0x03;
        public static readonly byte TYPE_ADDR_SET_COMMIT = 0x04;
        public static readonly byte TYPE_TILT_RESET = 0x06;
        public static readonly byte TYPE_TILT_RESET_COMMIT = 0x07;
        public static readonly byte TYPE_REGISTER = 0xFA;
        public static readonly byte TYPE_HEART_BEAT = 0xF9;


        public DatagramState State
        {
            get; set;
        }

        public int Length
        {
            get; set;
        }

        public byte Type
        {
            get; set;
        }

        public byte[] Data
        {
            get; set;
        }

        public byte Checksum
        {
            get; set;
        }

        public byte[] Bytes
        {
            get; set;
        }

        public IPEndPoint From { get; set; }
        
        public override string ToString()
        {
            string data = this.Data == null ? "" : BitConverter.ToString(this.Data);
            return $"[{this.State.ToString("G")}] - [LENGTH:{this.Length}] - [TYPE:{this.Type}] - [DATA:{data}]";
        }
    }
}
