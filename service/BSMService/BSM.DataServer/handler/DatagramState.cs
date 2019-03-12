using System;
using System.Collections.Generic;
using System.Text;

namespace BSM.DataServer
{
    public enum DatagramState
    {
        OK,
        InvalidLength,
        InvalidHeader,
        InvalidType
    }
}
