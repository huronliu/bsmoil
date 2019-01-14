using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BSM.Common.Model
{
    public class Coordinator
    {
        [Required]
        public long StationId { get; set; }

        [Key]
        public string Address { get; set; }

        public string Name { get; set; }

        [ForeignKey("StationId")]
        public Station Station { get; set; }

        public List<StationData> StationDatas { get; set; }
    }
}
