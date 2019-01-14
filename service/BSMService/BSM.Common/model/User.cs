using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BSM.Common.Model
{
    public class User
    {
        [Key]
        public long Id { get; set; }


        public string LoginID { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Department { get; set; }

        public string Company { get; set; }
        
        public bool IsAdmin { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public DateTime PasswordChangedAt { get; set; }

        public DateTime LastLoginAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
