using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api.config
{
    [Serializable]
    public class Position
    {
        [JsonProperty("lat")]
        public float Lat
        {
            get;set;
        }

        [JsonProperty("lng")]
        public float Lng
        {
            get;set;
        }
    }
}
