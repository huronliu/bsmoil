using System;
using System.Collections.Generic;
using System.Text;

namespace BSM.DataServer
{
    enum DatagramState
    {
        OK,
        InvalidLength,
        InvalidHeader,
        InvalidType
    }
}
