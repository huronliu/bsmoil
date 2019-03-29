using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSM.Api.config;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSM.Api.Controllers
{
    [Route("api/cities")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class CityController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<City>> GetAll()
        {
            return CitiesConfig.getProvinceList();
        }
    }
}
