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
using Sirius.Models.Dtos;

namespace Sirius.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/invoice")]
    public class InvoiceController : Controller
    {
        private SiriusService _siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public InvoiceController(IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _siriusService = new SiriusService(new UnitOfWork());
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// GET: api/invoice
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var invoices = _siriusService.GetAllInvoices();
            if (invoices != null)
            {
                return Ok(invoices);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/invoice/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var invoice = _siriusService.GetInvoiceById(id);
            if (invoice != null)
            {
                return Ok(invoice);
            }
            return NotFound();
        }

        /// <summary>
        /// POST: api/invoice
        /// </summary>
        /// <param name="register"></param>
        [HttpPost]
        public IActionResult Post([FromBody]Invoice invoice)
        {
            var result = _siriusService.AddInvoice(invoice);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// PUT: api/invoice/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]Invoice invoice)
        {
            var result = _siriusService.UpdateInvoice(id, invoice);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// DELETE: api/invoice/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_siriusService.DeleteInvoiceById(id))
            {
                return Ok();
            }
            return new BadRequestResult();
        }
    }
}
