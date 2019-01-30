using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api
{
    public class UserUpdateRequest
    {
        public string LoginID { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Department { get; set; }

        public string Company { get; set; }

        public bool? IsAdmin { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }
    }
}
