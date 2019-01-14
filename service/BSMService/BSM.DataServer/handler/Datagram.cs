using System;
using System.Collections.Generic;
using System.Text;

namespace BSM.DataServer
{
    class Datagram
    {
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

        public override string ToString()
        {
            string data = this.Data == null ? "" : BitConverter.ToString(this.Data);
            return $"[{this.State.ToString("G")}] - [LENGTH:{this.Length}] - [TYPE:{this.Type}] - [DATA:{data}]";
        }
    }
}
