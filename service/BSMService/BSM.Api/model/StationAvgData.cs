using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api.model
{
    public class StationAvgData
    {
        public long StationId { get; set; }

        public int SeqId { get; set; }

        public float Tilt_Avg_X { get; set; }

        public float Tilt_Avg_Y { get; set; }

        public string Date { get; set; }


    }
}
