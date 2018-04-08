using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sirius.Migrations
{
    public partial class AddInvoiceTypeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Factor",
                table: "Invoices");

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceTypeId",
                table: "Invoices",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "InvoiceTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Factor = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceTypes");

            migrationBuilder.DropColumn(
                name: "InvoiceTypeId",
                table: "Invoices");

            migrationBuilder.AddColumn<short>(
                name: "Factor",
                table: "Invoices",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
