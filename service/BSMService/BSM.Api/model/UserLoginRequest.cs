using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api
{
    public class UserLoginRequest
    {
        [Required]
        public string LoginID { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
