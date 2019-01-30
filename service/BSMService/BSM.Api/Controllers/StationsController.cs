using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BSM.Common.DB;
using BSM.Common.Model;
using BSM.Common.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BSM.Api.Controllers
{
    [Route("api/stations")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class StationsController : Controller
    {
        private readonly BSMContext _context;

        public StationsController(BSMContext context)
        {
            _context = context;
        }

        // GET: api/stations
        [HttpGet]
        public ActionResult<IEnumerable<Station>> Get()
        {
            return _context.Stations.ToList();
        }

        // GET api/stations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> Get(long id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            return station;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<Station>> Post([FromBody]StationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request Body can not be null");
            }
            if (await _context.Stations.AnyAsync(
                    st => st.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest($"{request.Name} already existed");
            }

            Station station = new Station();

            station.Name = request.Name;
            station.City = request.City;
            station.Lat = request.Lat;
            station.Lng = request.Lng;
            station.Province = request.Province;
            station.Tag = request.Tag;
            
            station.Disabled = false;
            station.CreatedAt = DateTime.Now;

            _context.Stations.Add(station);
            await _context.SaveChangesAsync();

            return station;
        }

        // PUT api/<controller>/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Station>> Put(long id, [FromBody]StationRequest updated)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null) return NotFound();

            if (string.IsNullOrWhiteSpace(updated.Name))
            {
                if (await _context.Stations.AnyAsync(st => 
                    st.Name.Equals(updated.Name, StringComparison.OrdinalIgnoreCase) &&
                    st.Id != id))
                {
                    return BadRequest($"Name '{updated.Name}' already exist");
                }
                station.Name = updated.Name;
            }

            if (updated.Tag != null)
            {
                station.Tag = updated.Tag;
            }
            if (updated.Lat.HasValue)
            {
                station.Lat = updated.Lat;
            }
            if (updated.Lng.HasValue)
            {
                station.Lng = updated.Lng;
            }
            if (updated.City != null)
            {
                station.City = updated.City;
            }
            if (updated.Province != null)
            {
                station.Province = updated.Province;
            }
            if (updated.Disabled.HasValue)
            {
                station.Disabled = updated.Disabled.Value;
            }
            

            await _context.SaveChangesAsync();

            return station;
        }
        
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        [HttpGet("{id}/coordinators")]
        public async Task<ActionResult<List<Coordinator>>> GetCoordinators(long id)
        {
            var stations = await (from cor in _context.Coordinators
                            where cor.StationId == id
                            orderby cor.SeqId
                            select cor).ToListAsync();
            return stations;
        }

        [HttpPost("{id}/coordinators/{seqid}")]
        public async Task<ActionResult<Coordinator>> CreateCoordinator(
            long id, int seqid,
            [FromBody] CoordinatorRequest request)
        {
            if (await _context.Coordinators.AnyAsync(co => 
                co.StationId == id &&
                co.SeqId == seqid))
            {
                return BadRequest($"Coordinator already exist");
            }

            Coordinator coordinator = new Coordinator();
            coordinator.StationId = id;
            coordinator.SeqId = seqid;
            coordinator.Name = request.Name;
            coordinator.Address = request.Address;
            coordinator.IPHost = request.IPHost;
            coordinator.IPPort = request.IPPort;
            coordinator.Disabled = false;
            coordinator.CreatedAt = DateTime.Now;

            _context.Coordinators.Add(coordinator);
            await _context.SaveChangesAsync();

            return coordinator;
        }

        [HttpPatch("{id}/coordinators/{seqid}")]
        public async Task<ActionResult<Coordinator>> UpdateCoordinator(
            long id, int seqid, 
            [FromBody] CoordinatorRequest request)
        {
            var coordinator = await _context.Coordinators.FirstOrDefaultAsync(co =>
                co.StationId == id && co.SeqId == seqid);
            if (coordinator == null)
            {
                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                coordinator.Name = request.Name;
            }
            if (string.IsNullOrWhiteSpace(request.Address))
            {
                coordinator.Address = request.Address;
            }
            if (request.IPHost != null)
            {
                coordinator.IPHost = request.IPHost;
            }
            if (request.IPPort.HasValue)
            {
                coordinator.IPPort = request.IPPort;
            }
            if (request.Disabled.HasValue)
            {
                coordinator.Disabled = request.Disabled.Value;
            }
            await _context.SaveChangesAsync();
            return coordinator;
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<List<StationComment>>> GetComments(long id)
        {
            var comments = await (from comment in _context.StationComments
                                  where comment.StationId == id
                                  orderby comment.CommentAt 
                                  select comment).ToListAsync();
            return comments;
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult<StationComment>> AddComment(long id, [FromBody] CommentRequest request)
        {
            StationComment comment = new StationComment();
            comment.Comment = request.Comment;
            comment.CommentAt = DateTime.Now;
            comment.StationId = id;

            _context.StationComments.Add(comment);
            await _context.SaveChangesAsync();

            return comment;
        }
    }
}
