using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BSM.Api.model;
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
            [FromQuery(Name = "start")] DateTime startTime, 
            [FromQuery(Name = "end")] DateTime endTime, 
            [FromQuery(Name = "index")] int? pageIndex, 
            [FromQuery(Name = "count")] int? pageCount)
        {
            if (!pageIndex.HasValue) pageIndex = 1;
            if (!pageCount.HasValue) pageCount = 20;

            var query = from d in _context.StationDatas
                        where d.StationId == stationId && d.SeqId == seqid && d.ReceivedAt >= startTime && d.ReceivedAt <= endTime
                        orderby d.ReceivedAt descending
                        select d;

            var result = query
                .Skip(pageCount.Value * (pageIndex.Value - 1))
                .Take(pageCount.Value)
                .ToList();

            return result;
        }

        [HttpGet("avg")]
        public IEnumerable<StationAvgData> GetAvgData(
            [FromQuery(Name = "stationid")][Required] long stationId,
            [FromQuery(Name = "seqid")] int? seqid,
            [FromQuery(Name = "start")] DateTime startDay, 
            [FromQuery(Name = "end")] DateTime endDay)
        {
            var query = from d in _context.StationDatas
                        where d.StationId == stationId && d.SeqId == seqid && d.ReceivedAt >= startDay && d.ReceivedAt <= endDay
                        group d by new
                        {
                            d.ReceivedAt.Value.Date
                        } into g
                        select new StationAvgData {
                            StationId = stationId,
                            SeqId = seqid.Value,
                            Date = g.Key.Date.ToString("yyyy-MM-dd"),
                            Tilt_Avg_X = g.Average(sd => (float)(sd.Tilt1_X_Degree + (float)sd.Tilt1_X_Minute / 60 + (float)sd.Tilt1_X_Second / 3600)),
                            Tilt_Avg_Y = g.Average(sd => (float)(sd.Tilt1_Y_Degree + (float)sd.Tilt1_Y_Minute / 60 + (float)sd.Tilt1_Y_Second / 3600))
                        };

            return query.ToList();
        }

        [HttpGet("latest")]
        public StationData GetLatest(
            [FromQuery(Name = "stationid")][Required] long stationId, 
            [FromQuery(Name = "seqid")][Required] int seqid)
        {
            var data = _context.StationDatas.Where(sd => sd.StationId == stationId && sd.SeqId == seqid)
                .OrderByDescending(sd => sd.ReceivedAt)
                .FirstOrDefault();
            return data;
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
