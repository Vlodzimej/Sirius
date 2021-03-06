﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using Sirius.Models;
using Sirius.Models.Dtos;
using Sirius.DAL;
using Sirius.Services;
using Sirius.Helpers;
using System.Collections.Generic;
using Sirius.Extends.Filters;

namespace Sirius.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/item")]
    public class ItemController : Controller
    {
        private SiriusService _siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ItemController(IMapper mapper, IOptions<AppSettings> appSettings, UnitOfWork unitOfWork)
        {
            _siriusService = new SiriusService(unitOfWork);
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// GET: api/item/filter
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery] ItemFilter filter)
        {
            var items = _siriusService.GetItemsByFilter(filter);
            if (items != null)
            {
                return Ok(items);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/item/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var item = _siriusService.GetItemById(id);
            if (item != null)
            {
                return Ok(item);
            }
            return NotFound();
        }

        /// <summary>
        /// POST: api/item
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Post([FromBody]ItemSaveDto item)
        {
            var result = _siriusService.AddItem(item);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// PUT: api/item/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Put(Guid id, [FromBody]Item item)
        {
            var result = _siriusService.UpdateItem(id, item);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// DELETE: api/item/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Guid id)
        {
            var result = _siriusService.DeleteItemById(id);
            if (result == id.ToString())
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
