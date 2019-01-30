using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BSM.Common.Model
{
    public class Alert
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long StationId { get; set; }
        
        public int? SeqId { get; set; }

        public byte[] Data { get; set; }

        public string State { get; set; }

        public string Resolution { get; set; }

        public string Comment { get; set; }

        public long? ResolvedBy { get; set; }

        public DateTime? ResolveAt { get; set; }

        [Required]
        public DateTime ReceivedAt { get; set; }

        [ForeignKey("StationId")]
        public Station Station { get; set; }

        [ForeignKey("ResolvedBy")]
        public User ResolvedUser { get; set; }
    }
}
