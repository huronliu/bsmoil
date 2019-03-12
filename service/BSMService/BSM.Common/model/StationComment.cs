using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BSM.Common.Model
{
    public class StationComment
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long StationId { get; set; }

        public string Comment { get; set; }

        public long? UserId { get; set; }

        public DateTime CommentAt { get; set; }


        [ForeignKey("StationId")]
        public Station Station { get; set; }

    }
}
