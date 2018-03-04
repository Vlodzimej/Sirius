using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sirius.Models;
using Sirius.BLL;
using Sirius.DAL;
using Microsoft.AspNetCore.Authorization;

namespace Sirius.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Dimension")]
    public class DimensionController : Controller
    {
        private SiriusService siriusService;

        DimensionController()
        {
            siriusService = new SiriusService(new UnitOfWork());
        }
        // GET: api/Dimension
        [HttpGet]
        public IActionResult Get()
        {
            var dimensions = siriusService.GetAllDimension();
            return Ok(dimensions);
        }

        // GET: api/Dimension/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(Guid id)
        {
            var dimension = siriusService.GetDimensionById(id);
            return Ok(dimension);
        }
        
        // POST: api/Dimension
        [HttpPost]
        public void Post([FromBody]Dimension dimension)
        {
            siriusService.AddDimension(dimension);
        }
        
        // PUT: api/Dimension/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            siriusService.DeleteDimensionById(id);
        }
    }
}
