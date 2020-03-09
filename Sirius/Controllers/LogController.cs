using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using Sirius.DAL;
using Sirius.Services;
using Sirius.Helpers;
using Sirius.Extends.Filters;

namespace Sirius.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/log")]
    public class LogController : Controller
    {
        private SiriusService _siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public LogController(IMapper mapper, IOptions<AppSettings> appSettings, UnitOfWork unitOfWork)
        {
            _siriusService = new SiriusService(unitOfWork);
            _mapper = mapper;
            _appSettings = appSettings.Value;

        }

        /// <summary>
        /// GET: api/log
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var logs = _siriusService.GetLogs();
            if (logs != null)
            {
                return Ok(logs);
            }
            return NotFound();
        }
    }
}