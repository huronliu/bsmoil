using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api
{
    public class UserRegisterRequest
    {
        [Required]
        public string LoginID { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
