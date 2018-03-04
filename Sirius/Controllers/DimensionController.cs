using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sirius.Models;
using Sirius.DAL;
using Sirius.BLL;
using Sirius.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.Extensions.Options;
using Sirius.Dtos;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Sirius.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/dimension")]

    public class DimensionController : Controller
    {
        private SiriusService siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public DimensionController(IMapper mapper, IOptions<AppSettings> appSettings)
        {
            siriusService = new SiriusService(new UnitOfWork());
            _mapper = mapper;
            _appSettings = appSettings.Value;
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
