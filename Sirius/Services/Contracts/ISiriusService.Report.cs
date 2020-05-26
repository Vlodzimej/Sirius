using System.Collections.Generic;
using OfficeOpenXml;
using Sirius.Helpers;
using Sirius.Models.Dtos;

namespace Sirius.Services
{
    partial interface ISiriusService
    {
        ExcelPackage CreateExcelPackage(List<BatchGroup> batches);
        ExcelPackage CreateCommonReport(List<ReportItemDto> reportItems);
    }
}
