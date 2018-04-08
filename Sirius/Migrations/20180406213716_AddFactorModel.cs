using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sirius.Migrations
{
    public partial class AddFactorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceTypeId",
                table: "Invoices",
                newName: "TypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "DomainId",
                table: "Settings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_DomainId",
                table: "Settings",
                column: "DomainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Domains_DomainId",
                table: "Settings",
                column: "DomainId",
                principalTable: "Domains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Domains_DomainId",
                table: "Settings");

            migrationBuilder.DropTable(
                name: "Domains");

            migrationBuilder.DropIndex(
                name: "IX_Settings_DomainId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Invoices",
                newName: "InvoiceTypeId");
        }
    }
}
