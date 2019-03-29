using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api.config
{
    [Serializable]
    public class City
    {
        [JsonProperty("citycode")]
        public string CityCode
        {
            get;set;
        }

        [JsonProperty("adcode")]
        public string AdCode
        {
            get;set;
        }

        [JsonProperty("name")]
        public string Name
        {
            get;set;
        }

        [JsonProperty("center")]
        public Position Center
        {
            get;set;
        }

        [JsonProperty("level")]
        public string Level
        {
            get; set;
        }

        [JsonProperty("districtList")]
        public City[] DistrictList
        {
            get;set;
        }
    }
}
