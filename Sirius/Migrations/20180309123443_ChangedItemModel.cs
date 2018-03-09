using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sirius.Migrations
{
    public partial class ChangedItemModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "parentId",
                table: "Categories",
                newName: "ParentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "Categories",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DimensionId",
                table: "Items",
                column: "DimensionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Dimensions_DimensionId",
                table: "Items",
                column: "DimensionId",
                principalTable: "Dimensions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Dimensions_DimensionId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_DimensionId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Categories",
                newName: "parentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "parentId",
                table: "Categories",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
