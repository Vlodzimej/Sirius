using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using Sirius.Models;
using Sirius.DAL;
using Sirius.Services;
using Sirius.Helpers;
using System.IO;

namespace Sirius.Controllers
{
    [Authorize(Roles = "admin")]
    [Produces("application/json")]
    [Route("api/report")]
    public class ReportController : Controller
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private SiriusService _siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ReportController(IMapper mapper, IOptions<AppSettings> appSettings, UnitOfWork unitOfWork)
        {
            _siriusService = new SiriusService(unitOfWork);
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        /// <summary>
        /// GET: getexcelreport
        /// Отчет в Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet("getexcelreport")]
        public FileStreamResult GetExcelReport()
        {
            var contentType = "application/octet-stream";
            var fileName = "report.xlsx";
            var stream = new MemoryStream();
            stream.Position = 0;
            /*

            return File(fileDownloadName, XlsxContentType, fileDownloadName);*/

            return File(stream, contentType, fileName);
        }
    }
}
