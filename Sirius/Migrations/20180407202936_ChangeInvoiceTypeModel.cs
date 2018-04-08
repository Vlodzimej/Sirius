using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sirius.Migrations
{
    public partial class ChangeInvoiceTypeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceTypeId",
                table: "Invoices",
                newName: "TypeId");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "InvoiceTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "InvoiceTypes");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Invoices",
                newName: "InvoiceTypeId");
        }
    }
}
