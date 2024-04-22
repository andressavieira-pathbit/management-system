using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.System.Infrastructure.Migrations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class UpdateEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_Users_UserId",
            table: "Orders");

        migrationBuilder.DropForeignKey(
            name: "FK_Products_Users_UserId",
            table: "Products");

        migrationBuilder.DropIndex(
            name: "IX_Products_UserId",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Products");

        migrationBuilder.RenameColumn(
            name: "UserId",
            table: "Orders",
            newName: "CustomerId");

        migrationBuilder.RenameIndex(
            name: "IX_Orders_UserId",
            table: "Orders",
            newName: "IX_Orders_CustomerId");

        migrationBuilder.AddColumn<Guid>(
            name: "CustomerId",
            table: "Users",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_Users_CustomerId",
            table: "Users",
            column: "CustomerId");

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_Customers_CustomerId",
            table: "Orders",
            column: "CustomerId",
            principalTable: "Customers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Customers_CustomerId",
            table: "Users",
            column: "CustomerId",
            principalTable: "Customers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_Customers_CustomerId",
            table: "Orders");

        migrationBuilder.DropForeignKey(
            name: "FK_Users_Customers_CustomerId",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_CustomerId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "CustomerId",
            table: "Users");

        migrationBuilder.RenameColumn(
            name: "CustomerId",
            table: "Orders",
            newName: "UserId");

        migrationBuilder.RenameIndex(
            name: "IX_Orders_CustomerId",
            table: "Orders",
            newName: "IX_Orders_UserId");

        migrationBuilder.AddColumn<Guid>(
            name: "UserId",
            table: "Products",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_Products_UserId",
            table: "Products",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_Users_UserId",
            table: "Orders",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Products_Users_UserId",
            table: "Products",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
