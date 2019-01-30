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
        [Key]
        public long StationId { get; set; }

        [Required]
        [Key]
        public int SeqId { get; set; }

        public string Address { get; set; }

        public String IPHost { get; set; }

        public int? IPPort { get; set; }

        public string Name { get; set; }

        public bool Disabled { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("StationId")]
        public Station Station { get; set; }

    }
}
