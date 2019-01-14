using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BSM.Common.Model
{
    public class UserPermission
    {
        [Key]
        public long UserId { get; set; }

        public string AllowedProvinces { get; set; }
        
        public bool CanEditUser { get; set; }

        public bool CanEditStation { get; set; }

        public bool CanViewReport { get; set; }

        public bool CanResolveAlert { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
