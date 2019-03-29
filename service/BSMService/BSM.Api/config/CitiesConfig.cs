using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api.config
{
    public class CitiesConfig
    {
        private static IDictionary<string, City> provinces = new Dictionary<string, City>();

        public static void LoadCitiesList()
        {
            try
            {
                City[] cities = JsonConvert.DeserializeObject<City[]>(File.ReadAllText("cities.json"));
                provinces.Clear();
                
                if (cities != null && cities.Length > 0)
                {
                    foreach(var city in cities)
                    {
                        provinces.Add(city.Name, city);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error when load city list from cities.json");
            }
        }

        public static List<City> getProvinceList()
        {
            return provinces.Values.ToList();
        }

        public static List<City> getCityList(string provinceName)
        {
            var city = provinces[provinceName];
            if (city != null)
            {
                return city.DistrictList.ToList();
            }
            return null;
        }
    }
}
