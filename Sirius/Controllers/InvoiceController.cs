using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using Sirius.Models;
using Sirius.DAL;
using Sirius.Services;
using Sirius.Helpers;
using Sirius.Extends.Filters;
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

        public InvoiceController(IMapper mapper, IOptions<AppSettings> appSettings, UnitOfWork unitOfWork)
        {
            _siriusService = new SiriusService(unitOfWork);
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// GET: api/invoice
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(InvoiceFilter filter)
        {
            var invoices = _siriusService.GetInvoices(filter);
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
            var invoice = _siriusService.GetInvoiceDetailDtoById(id);
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Guid id)
        {
            var result = _siriusService.DeleteInvoiceById(id);
            if (result != "")
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// POST: api/invoice/fix/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        [HttpPut("fix/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Fix(Guid id)
        {
            var result = _siriusService.FixInvoice(id);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        [HttpPut("{invoiceId}/vendor")]
        [Authorize(Roles = "admin")]
        public IActionResult ChangeVendor(Guid invoiceId, [FromQuery]Guid value)
        {
            if (invoiceId != null && value != null)
            {
                var result = _siriusService.ChangeVendor(invoiceId, value);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            return new BadRequestResult();
        }

        [HttpPut("{invoiceId}/name")]
        [Authorize(Roles = "admin")]
        public IActionResult ChangeName(Guid invoiceId, [FromQuery]string value)
        {
            if (value != null && value != "")
            {
                var result = _siriusService.ChangeName(invoiceId, value);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// GET: api/typebyid/4C070178-29FB-40A0-ACF9-10DD83641C51
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("type/id/{id}")]
        public IActionResult GetTypes(Guid id)
        {
            var type = _siriusService.GetInvoiceTypeByTypeId(id);
            if (type != null)
            {
                return Ok(type);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/typebyalias/arrival
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        [HttpGet("type/alias/{alias}")]
        public IActionResult GetTypeByAlias(string alias)
        {
            var type = _siriusService.GetInvoiceTypeByAlias(alias);
            if (type != null)
            {
                return Ok(type);
            }
            return NotFound();
        }

        /// <summary>
        /// GET:api/alltypes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("alltypes")]
        public IActionResult GetType(Guid id)
        {
            var type = _siriusService.GetInvoiceTypes();
            if (type != null)
            {
                return Ok(type);
            }
            return NotFound();
        }

        [HttpPut("comment/{invoiceId}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateComment(Guid invoiceId, [FromQuery]string value)
        {
            if (_siriusService.UpdateComment(invoiceId, value))
            {
                return Ok(true);
            }
            return NotFound();
        }



    }
}
