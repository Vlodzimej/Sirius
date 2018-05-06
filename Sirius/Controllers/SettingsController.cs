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
    [Route("api/settings")]
    public class SettingsController : Controller
    {
        private SiriusService _siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public SettingsController(IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _siriusService = new SiriusService(new UnitOfWork());
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        /// <summary>
        /// POST: dbreset
        /// Сброс базы данных
        /// </summary>
        /// <returns></returns>
        [HttpPost("dbreset")]
        public IActionResult DbReset()
        {
            _siriusService.DatabaseReset();
            return Ok();
        }


    }
}
