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
    [Route("api/vendor")]
    public class VendorController : Controller
    {
        private SiriusService _siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public VendorController(IMapper mapper, IOptions<AppSettings> appSettings, UnitOfWork unitOfWork)
        {
            _siriusService = new SiriusService(unitOfWork);
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// GET: api/vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var vendors = _siriusService.GetAllVendors() as List<Vendor>;
            if (vendors.Count > 0)
            {
                return Ok(vendors);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/vendor/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var vendor = _siriusService.GetVendorById(id);
            if (vendor != null)
            {
                return Ok(vendor);
            }
            return NotFound();
        }

        /// <summary>
        /// POST: api/vendor
        /// </summary>
        /// <param name="vendor"></param>
        [HttpPost]
        public IActionResult Post([FromBody]Vendor vendor)
        {
            var result = _siriusService.AddVendor(vendor);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// PUT: api/vendor/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]Vendor vendor)
        {
            var result = _siriusService.UpdateVendor(id, vendor);
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
            if (_siriusService.DeleteVendorById(id))
            {
                return Ok();
            }
            return new BadRequestResult();
        }
    }
}
