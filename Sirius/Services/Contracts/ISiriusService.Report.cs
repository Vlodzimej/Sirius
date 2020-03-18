using System.Collections.Generic;
using OfficeOpenXml;
using Sirius.Helpers;

namespace Sirius.Services
{
    partial interface ISiriusService
    {
        ExcelPackage CreateExcelPackage(List<BatchGroup> batches);
    }
}
