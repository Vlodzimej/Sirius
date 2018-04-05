using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sirius.Migrations
{
    public partial class InvoiceChanged2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Factor",
                table: "Invoices",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Factor",
                table: "Invoices");
        }
    }
}
