using BSM.Common.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using BSM.Common.Model;

namespace BSM.Common.service
{
    public class StationService
    {
        private readonly BSMContext _context;

        public StationService(BSMContext context)
        {
            _context = context;
        }

        public async Task<bool> IsNameExisted(string name)
        {
            return await _context.Stations.AnyAsync(st => st.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task CreateNew(Station station)
        {
            station.Disabled = false;
            station.CreatedAt = DateTime.Now;

            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
        }
    }
}
