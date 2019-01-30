using BSM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSM.Api
{
    public class UserLoginResponse
    {
        public string Token { get; set; }
        public User User { get; set; }

        public UserLoginResponse(User user, string token)
        {
            this.User = user;
            this.Token = token;
        }
    }
}
