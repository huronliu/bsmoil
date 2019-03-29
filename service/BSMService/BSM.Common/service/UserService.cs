using BSM.Common.DB;
using BSM.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSM.Common.service
{
    public class UserService
    {
        private readonly BSMContext _context;

        public UserService(BSMContext context)
        {
            _context = context;
        }

    }
}
