using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BSM.Common.DB;
using BSM.Common.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSM.Api.Controllers
{
    [Route("api/stationdata")]
    public class StationDataController : Controller
    {
        private readonly BSMContext _context;

        public StationDataController(BSMContext context)
        {
            _context = context;
        }

        // GET: api/stationdata
        [HttpGet]
        public IEnumerable<StationData> Get(
            [FromQuery(Name = "stationid")][Required] long stationId, 
            [FromQuery(Name = "seqid")] int? seqid,
            [FromQuery(Name = "start")] DateTime? startTime, 
            [FromQuery(Name = "end")] DateTime? endTime, 
            [FromQuery(Name = "index")] int? pageIndex, 
            [FromQuery(Name = "count")] int? pageCount)
        {
            if (!pageIndex.HasValue) pageIndex = 1;
            if (!pageCount.HasValue) pageCount = 20;

            return _context.StationDatas
                .Where(sd =>
                    sd.StationId == stationId &&
                    seqid.HasValue? sd.SeqId == seqid : true &&
                    startTime.HasValue ? sd.ReceivedAt >= startTime.Value : true &&
                    endTime.HasValue ? sd.ReceivedAt <= endTime.Value : true)
                .Skip(pageCount.Value * (pageIndex.Value - 1))
                .Take(pageCount.Value)
                .OrderByDescending(sd => sd.ReceivedAt)
                .ToList();
        }

        // POST api/stationdata
        [HttpPost]
        public async Task<ActionResult<StationData>> Post([FromBody]StationData data)
        {
            //verify stationId
            if (data.StationId <= 0)
            {
                return BadRequest("Invalid StationID");
            }
            //verify seqid
            if (data.SeqId <= 0)
            {
                return BadRequest("Invalid SeqID");
            }
            if (data.ReceivedAt == null)
            {
                data.ReceivedAt = DateTime.Now;
            }
            _context.StationDatas.Add(data);
            await _context.SaveChangesAsync();

            return data;
        }

    }
}
