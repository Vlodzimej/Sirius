using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using Sirius.Helpers;
using Sirius.Models.Dtos;

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


        public ExcelPackage CreateCommonReport(List<ReportItemDto> reportItems)
        {
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Клиника \"СИРИУС-ВЕТ\"";
            package.Workbook.Properties.Author = "";
            package.Workbook.Properties.Subject = "Отчет по остаткам";
            package.Workbook.Properties.Keywords = "";

            var worksheet = package.Workbook.Worksheets.Add("Остатки");

            //First add the headers
            worksheet.Cells[1, 1].Value = "Наименование";
            worksheet.Cells[1, 2].Value = "Приход";
            worksheet.Cells[1, 3].Value = "Расход";
            worksheet.Cells[1, 4].Value = "Остаток";
            worksheet.Cells[1, 5].Value = "Ед. изм.";

            //Add values
            var dataCellStyleName = "TableNumber";
            var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            var colNumb = 2;
            reportItems.ForEach(item =>
            {
                worksheet.Cells[colNumb, 1].Value = item.Name;
                worksheet.Cells[colNumb, 2].Value = item.Incoming;
                worksheet.Cells[colNumb, 3].Value = item.Consumption;
                worksheet.Cells[colNumb, 4].Value = item.Total;
                worksheet.Cells[colNumb, 5].Value = item.Dimension;

                worksheet.Cells[colNumb, 2].Style.Numberformat.Format = setAmountNumberFormat(item.Incoming);
                worksheet.Cells[colNumb, 3].Style.Numberformat.Format = setAmountNumberFormat(item.Consumption);
                worksheet.Cells[colNumb, 4].Style.Numberformat.Format = setAmountNumberFormat(item.Total);
                colNumb++;
            });

            using (ExcelRange Rng = worksheet.Cells[1, 1, 1, 5])
            {
                Rng.Style.Font.Size = 18;
                Rng.Style.Font.Bold = true;
                Rng.AutoFitColumns();
            }
            using (ExcelRange Rng = worksheet.Cells[2, 1, colNumb, 5])
            {
                Rng.Style.Font.Size = 14;
                Rng.AutoFitColumns();
            }

            return package;
        }

        private string setAmountNumberFormat(double value)
        {
            var numberformat = "#,##0.00";
            return value - Math.Round(value) == 0 ? "0" : numberformat;
        }
    }
}
