using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Sirius.Helpers;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        public ExcelPackage CreateExcelPackage(List<BatchGroup> batches)
        {
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Клиника \"СИРИУС-ВЕТ\"";
            package.Workbook.Properties.Author = "";
            package.Workbook.Properties.Subject = "Отчет по остаткам";
            package.Workbook.Properties.Keywords = "";


            var worksheet = package.Workbook.Worksheets.Add("Остатки");

            //First add the headers
            worksheet.Cells[1, 1].Value = "Наименование";
            worksheet.Cells[1, 2].Value = "Кол-во";
            worksheet.Cells[1, 3].Value = "Стоимость";
            worksheet.Cells[1, 4].Value = "";

            //Add values

            var numberformat = "#,##0";
            var dataCellStyleName = "TableNumber";
            var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            numStyle.Style.Numberformat.Format = numberformat;

            worksheet.Cells[2, 1].Value = 1000;
            worksheet.Cells[2, 2].Value = "Jon";
            worksheet.Cells[2, 3].Value = "M";
            worksheet.Cells[2, 4].Value = 5000;
            worksheet.Cells[2, 4].Style.Numberformat.Format = numberformat;

            worksheet.Cells[3, 1].Value = 1001;
            worksheet.Cells[3, 2].Value = "Graham";
            worksheet.Cells[3, 3].Value = "M";
            worksheet.Cells[3, 4].Value = 10000;
            worksheet.Cells[3, 4].Style.Numberformat.Format = numberformat;

            worksheet.Cells[4, 1].Value = 1002;
            worksheet.Cells[4, 2].Value = "Jenny";
            worksheet.Cells[4, 3].Value = "F";
            worksheet.Cells[4, 4].Value = 5000;
            worksheet.Cells[4, 4].Style.Numberformat.Format = numberformat;

            // Add to table / Add summary row
            var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: 4, toColumn: 4), "Data");
            tbl.ShowHeader = true;
            tbl.TableStyle = TableStyles.Dark9;
            tbl.ShowTotal = true;
            tbl.Columns[3].DataCellStyleName = dataCellStyleName;
            tbl.Columns[3].TotalsRowFunction = RowFunctions.Sum;
            worksheet.Cells[5, 4].Style.Numberformat.Format = numberformat;

            // AutoFitColumns
            worksheet.Cells[1, 1, 4, 4].AutoFitColumns();

            //worksheet.HeaderFooter.OddFooter.InsertPicture(
            //    new FileInfo(Path.Combine(webRootPath, "images", "captcha.jpg")),
            //    PictureAlignment.Right);

            return package;
        }

    }
}
