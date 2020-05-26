
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using Sirius.DAL;
using Sirius.Services;
using Sirius.Helpers;
using System.IO;

namespace Sirius.Controllers
{
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
        public IActionResult GetExcelReport()
        {
            try
            {
                // Путь к файлу
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "report.xlsx");
                // Тип файла - content-type
                string fileType = XlsxContentType;
                // Имя файла - необязательно
                string fileName = "report.xlsx";
                return PhysicalFile(filePath, fileType, fileName);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("getcommonreport")]
        public IActionResult GetCommonReport()
        {
            var result = _siriusService.GetReportItems();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
