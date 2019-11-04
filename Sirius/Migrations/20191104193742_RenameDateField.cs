using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sirius.Migrations
{
    public partial class RenameDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "StorageRegisters",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Invoices",
                newName: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "StorageRegisters",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Invoices",
                newName: "CreateDate");
        }
    }
}
