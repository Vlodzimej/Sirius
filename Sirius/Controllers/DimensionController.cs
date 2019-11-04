using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using Sirius.Models;
using Sirius.DAL;
using Sirius.Services;
using Sirius.Helpers;
using System.Collections.Generic;

namespace Sirius.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/dimension")]
    public class DimensionController : Controller
    {
        private SiriusService _siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public DimensionController(IMapper mapper, IOptions<AppSettings> appSettings, UnitOfWork unitOfWork)
        {
            _siriusService = new SiriusService(unitOfWork);
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// GET: api/Dimension
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var dimensions = _siriusService.GetAllDimension() as List<Dimension>;
            if (dimensions.Count > 0)
            {
                return Ok(dimensions);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/dimension/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var dimension = _siriusService.GetDimensionById(id);
            if (dimension != null)
            {
                return Ok(dimension);
            }
            return NotFound();
        }

        /// <summary>
        /// POST: api/Dimension
        /// </summary>
        /// <param name="dimension"></param>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Post([FromBody]Dimension dimension)
        {
            var result = _siriusService.AddDimension(dimension);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// PUT: api/Dimension/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Put(Guid id, [FromBody]Dimension dimension)
        {
            var result = _siriusService.UpdateDimension(id, dimension);
            if(result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// DELETE: api/ApiWithActions/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public void Delete(Guid id)
        {
            _siriusService.DeleteDimensionById(id);
        }
    }
}
