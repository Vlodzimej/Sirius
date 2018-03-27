using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sirius.Migrations
{
    public partial class RegistersContextRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Register_Invoices_InvoiceId",
                table: "Register");

            migrationBuilder.DropForeignKey(
                name: "FK_Register_Items_ItemId",
                table: "Register");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Register",
                table: "Register");

            migrationBuilder.RenameTable(
                name: "Register",
                newName: "Registers");

            migrationBuilder.RenameIndex(
                name: "IX_Register_ItemId",
                table: "Registers",
                newName: "IX_Registers_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Register_InvoiceId",
                table: "Registers",
                newName: "IX_Registers_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registers",
                table: "Registers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_Invoices_InvoiceId",
                table: "Registers",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_Items_ItemId",
                table: "Registers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registers_Invoices_InvoiceId",
                table: "Registers");

            migrationBuilder.DropForeignKey(
                name: "FK_Registers_Items_ItemId",
                table: "Registers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registers",
                table: "Registers");

            migrationBuilder.RenameTable(
                name: "Registers",
                newName: "Register");

            migrationBuilder.RenameIndex(
                name: "IX_Registers_ItemId",
                table: "Register",
                newName: "IX_Register_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Registers_InvoiceId",
                table: "Register",
                newName: "IX_Register_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Register",
                table: "Register",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Register_Invoices_InvoiceId",
                table: "Register",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Register_Items_ItemId",
                table: "Register",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
