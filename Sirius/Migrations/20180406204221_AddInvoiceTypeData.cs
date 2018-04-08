using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using Sirius.DAL;
using Sirius.Models;
using Sirius.Helpers;

namespace Sirius.Migrations
{
    public partial class AddInvoiceTypeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = new SiriusContext())
            {
                var invoiceType = new InvoiceType()
                {
                    Id = Types.InvoiceTypes.Arrival.Id,
                    Factor = 1,
                    Name = "Приход",
                    Alias = "arrival"
                };
                context.InvoiceTypes.Add(invoiceType);

                invoiceType = new InvoiceType()
                {
                    Id = Types.InvoiceTypes.Consumption.Id,
                    Factor = (-1),
                    Name = "Расход",
                    Alias = "consumption"
                };
                context.InvoiceTypes.Add(invoiceType);

                invoiceType = new InvoiceType()
                {
                    Id = Types.InvoiceTypes.Writeoff.Id,
                    Factor = (-1),
                    Name = "Списание",
                    Alias = "writeoff"
                };
                context.InvoiceTypes.Add(invoiceType);

                context.SaveChanges();
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
