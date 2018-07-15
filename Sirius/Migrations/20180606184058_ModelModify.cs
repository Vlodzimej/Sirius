using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sirius.Migrations
{
    public partial class ModelModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "createDate",
                table: "StorageRegisters",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "isCountless",
                table: "Items",
                newName: "IsCountless");

            migrationBuilder.AddColumn<double>(
                name: "MinimumLimit",
                table: "Items",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumLimit",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "StorageRegisters",
                newName: "createDate");

            migrationBuilder.RenameColumn(
                name: "IsCountless",
                table: "Items",
                newName: "isCountless");
        }
    }
}
