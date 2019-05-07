using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hello.Profile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        // GET api/health
        [HttpGet]
        public ActionResult<dynamic> Get()
        {
            return new {
                Status = "Ok",
                DateTime = DateTime.UtcNow,
                Version = "1.0.0"
            };
        }
    }
}
