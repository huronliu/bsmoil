using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api
{
    public class CoordinatorRequest
    {
        public string Address { get; set; }

        public String IPHost { get; set; }

        public int? IPPort { get; set; }

        public string Name { get; set; }

        public bool? Disabled { get; set; }
    }
}
