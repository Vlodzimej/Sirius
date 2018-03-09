﻿using System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using AutoMapper;

using Sirius.Models;
using Sirius.DAL;
using Sirius.Services;
using Sirius.Helpers;

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

        /// <summary>
        /// GET: api/Dimension
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var dimensions = siriusService.GetAllDimension();
            return Ok(dimensions);
        }

        /// <summary>
        /// GET: api/Dimension/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetDimension")]
        public IActionResult Get(Guid id)
        {
            var dimension = siriusService.GetDimensionById(id);
            return Ok(dimension);
        }

        /// <summary>
        /// POST: api/Dimension
        /// </summary>
        /// <param name="dimension"></param>
        [HttpPost]
        public void Post([FromBody]Dimension dimension)
        {
            siriusService.AddDimension(dimension);
        }

        /// <summary>
        /// PUT: api/Dimension/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]Dimension value)
        {
            var dimension = siriusService.UpdateDimension(id, value);
            if(dimension != null)
            {
                return Ok(dimension);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// DELETE: api/ApiWithActions/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            siriusService.DeleteDimensionById(id);
        }
    }
}
