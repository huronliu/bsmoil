using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api
{
    public class StationRequest
    {
        public string Name { get; set; }

        public string Tag { get; set; }

        public float? Lng { get; set; }

        public float? Lat { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public float? Height { get; set; }

        public bool? Disabled { get; set; }
    }
}
