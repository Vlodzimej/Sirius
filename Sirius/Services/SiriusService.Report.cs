using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Sirius.Helpers;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        public ExcelPackage CreateExcelPackage(List<BatchGroup> batchGroups)
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
            worksheet.Cells[1, 4].Value = "Сумма";

            //Add values

            var numberformat = "#,##0.00";
            var dataCellStyleName = "TableNumber";
            var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            var colNumb = 2;
            batchGroups.ForEach(group =>
            {
                if (group.Batches.Count() == 0) return;
                worksheet.Cells[colNumb, 1].Value = group.Name;
                colNumb++;
                group.Batches.ToList().ForEach(batch =>
                {
                    worksheet.Cells[colNumb, 2].Value = batch.Amount;
                    worksheet.Cells[colNumb, 3].Value = batch.Cost;
                    worksheet.Cells[colNumb, 4].Value = batch.Amount * (Double)batch.Cost;

                    worksheet.Cells[colNumb, 2].Style.Numberformat.Format = batch.Amount - Math.Round(batch.Amount) == 0 ? "0" : numberformat;
                    worksheet.Cells[colNumb, 3].Style.Numberformat.Format = numberformat;
                    worksheet.Cells[colNumb, 4].Style.Numberformat.Format = numberformat;
                    colNumb++;
                });
            });

            using (ExcelRange Rng = worksheet.Cells[1, 1, 1, 4])
            {
                Rng.Style.Font.Size = 16;
                Rng.Style.Font.Bold = true;
                Rng.AutoFitColumns();
            }

            // Add to table / Add summary row
            //var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: 4, toColumn: 4), "Data");
            //tbl.ShowHeader = true;
            //tbl.TableStyle = TableStyles.Dark9;
            //tbl.ShowTotal = true;
            //tbl.Columns[3].DataCellStyleName = dataCellStyleName;
            //tbl.Columns[3].TotalsRowFunction = RowFunctions.Sum;
            //worksheet.Cells[5, 4].Style.Numberformat.Format = numberformat;

            // AutoFitColumns
            //worksheet.Cells[1, 1, 4, 4].AutoFitColumns();

            //worksheet.HeaderFooter.OddFooter.InsertPicture(
            //    new FileInfo(Path.Combine(webRootPath, "images", "captcha.jpg")),
            //    PictureAlignment.Right);

            return package;
        }
    }
}
