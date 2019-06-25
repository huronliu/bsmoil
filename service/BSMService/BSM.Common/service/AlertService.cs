using BSM.Common.DB;
using BSM.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BSM.Common.service
{
    public class AlertService
    {
        private readonly BSMContext _context;

        public AlertService(BSMContext context)
        {
            _context = context;
        }

        AlertSetting _setting; 
        public AlertSetting Setting
        {
            get
            {
                if (_setting == null)
                {
                    _setting = _context.AlertSettings.FirstOrDefault();
                }
                return _setting;
            }
        }

        public void refreshSetting()
        {
            this._setting = null;
        }
    }
}
