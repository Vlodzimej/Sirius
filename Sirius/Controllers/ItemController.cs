using System;

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
    [Route("api/item")]
    public class ItemController : Controller
    {
        private SiriusService siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ItemController(IMapper mapper, IOptions<AppSettings> appSettings)
        {
            siriusService = new SiriusService(new UnitOfWork());
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// GET: api/item
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
             var items = siriusService.GetAllItems();
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
            var item = siriusService.GetItemById(id);
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
        public IActionResult Post([FromBody]Item item)
        {
            var result = siriusService.AddItem(item);
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
        public IActionResult Put(Guid id, [FromBody]Item value)
        {
            var item = siriusService.UpdateItem(id, value);
            if (item != null)
            {
                return Ok(item);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// DELETE: api/item/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (siriusService.DeleteItemById(id))
            {
                return Ok();
            }
            return new BadRequestResult();
        }
    }
}
