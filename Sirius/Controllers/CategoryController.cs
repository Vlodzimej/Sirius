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
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private SiriusService siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CategoryController(IMapper mapper, IOptions<AppSettings> appSettings)
        {
            siriusService = new SiriusService(new UnitOfWork());
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// GET: api/category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var categories = siriusService.GetAllCategory() as List<Category>;
            if (categories.Count > 0)
            {
                return Ok(categories);
            }
            return NotFound();
        }

        /// <summary>
        /// GET: api/category/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var category = siriusService.GetCategoryById(id);
            if (category != null)
            {
                return Ok(category);
            }
            return NotFound();
        }

        /// <summary>
        /// POST: api/category
        /// </summary>
        /// <param name="category"></param>
        [HttpPost]
        public IActionResult Post([FromBody]Category category)
        {
            var result = siriusService.AddCategory(category);
            if (result != null)
            {
                return Ok(result);
            }
            return new BadRequestResult();
        }

        /// <summary>
        /// PUT: api/category/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]Category category)
        {
            var result = siriusService.UpdateCategory(id, category);
            if (result != null)
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
        public IActionResult Delete(Guid id)
        {
            if (siriusService.DeleteCategoryById(id))
            {
                return Ok();
            }
            return new BadRequestResult();
        }
    }
}
