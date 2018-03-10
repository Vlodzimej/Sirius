﻿using System;
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
    [Route("api/register")]
    public class RegisterController : Controller
    {
        private SiriusService siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public RegisterController(IMapper mapper, IOptions<AppSettings> appSettings)
        {
            siriusService = new SiriusService(new UnitOfWork());
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// GET: api/register
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var registers = siriusService.GetAllRegisters() as List<Register>;
            if (registers.Count > 0)
            {
                return Ok(registers);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/register/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var register = siriusService.GetRegisterById(id);
            if (register != null)
            {
                return Ok(register);
            }
            return NotFound();
        }

        /// <summary>
        /// POST: api/register
        /// </summary>
        /// <param name="register"></param>
        [HttpPost]
        public IActionResult Post([FromBody]Register register)
        {
            var result = siriusService.AddRegister(register);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// PUT: api/register/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]Register register)
        {
            var result = siriusService.UpdateRegister(id, register);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// DELETE: api/vendor/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (siriusService.DeleteVendorById(id))
            {
                return Ok();
            }
            return new BadRequestResult();
        }
    }
}