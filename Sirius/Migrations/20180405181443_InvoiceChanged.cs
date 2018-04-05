using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sirius.Migrations
{
    public partial class InvoiceChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRecorded",
                table: "Invoices",
                newName: "IsFixed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFixed",
                table: "Invoices",
                newName: "IsRecorded");
        }
    }
}
