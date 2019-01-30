using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BSM.Common.Model
{
    public class Station
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Tag { get; set; }

        public float? Lng { get; set; }

        public float? Lat { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public bool Disabled { get; set; }

        public long? CreateBy { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }

        List<Coordinator> Coordinators { get; set; }
        List<StationComment> Comments { get; set; }
    }
}
