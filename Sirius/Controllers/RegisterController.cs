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
using Sirius.Extends.Filters;

namespace Sirius.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/register")]
    public class RegisterController : Controller
    {
        private SiriusService _siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public RegisterController(IMapper mapper, IOptions<AppSettings> appSettings, UnitOfWork unitOfWork)
        {
            _siriusService = new SiriusService(unitOfWork);
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// GET: api/register
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(MetaFilter filter)
        {
            var registers = _siriusService.GetRegistersByFilter(filter);
            if (registers != null)
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
            var register = _siriusService.GetRegisterById(id);
            if (register != null)
            {
                return Ok(register);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/register/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("item/{itemId}")]  
        public IActionResult GetByItemId(Guid itemId)
        {
            var response = _siriusService.GetBatchesByItemId(itemId);
            if (response != null)
            {
                return Ok(response.Result);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/register/type/arrive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("type/{typeAlias}")]
        public IActionResult GetByItemId(string typeAlias)
        {
            var registers = _siriusService.GetRegisterByTypeAlias(typeAlias);
            if (registers != null)
            {
                return Ok(registers);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/register/batches
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("batches")]
        public IActionResult GetBatches(BatchFilter filter)
        {
            var batches = _siriusService.GetBatches(filter);
            if (batches != null)
            {
                return Ok(batches);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/register/batches
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("batch")]
        public IActionResult GetBatch(BatchFilter filter)
        {
            var batches = _siriusService.GetBatch(filter);
            if (batches != null)
            {
                return Ok(batches);
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
            var result = _siriusService.AddRegister(register);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// POST: api/register   ??????????? ПРОВЕРИТЬ ИСПОЛЬЗОВАНИЕ. НУЖЕН ЛИ ЭТОТ МЕТОД?
        /// </summary>
        /// <param name="register"></param>
        [HttpPost("registers")]
        public IActionResult PostArray([FromBody]Register[] registers)
        {
            var result = _siriusService.AddRegisters(registers);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// POST: api/register/copy?sourceId=null&destinationId=null
        /// </summary>
        /// <param name="register"></param>
        [HttpPost("copy")]
        public IActionResult CopyRegisterArray([FromQuery]Guid sourceId,[FromQuery]Guid destinationId)
        {
            var result = _siriusService.CopyRegisters(sourceId, destinationId);
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
        [HttpPut("{registerId}")]
        public IActionResult Put(Guid registerId, [FromBody]Register register)
        {
            var result = _siriusService.UpdateRegister(registerId, register);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// DELETE: api/register/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_siriusService.DeleteRegisterById(id))
            {
                return Ok();
            }
            return new BadRequestResult();
        }

    }
}

