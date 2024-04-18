using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Management.System.Infrastructure.Migrations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class RemoveDoubleFkFromOrder : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_Customers_CustomerId",
            table: "Orders");

        migrationBuilder.DropForeignKey(
            name: "FK_Orders_Products_ProductId",
            table: "Orders");

        migrationBuilder.DropIndex(
            name: "IX_Orders_CustomerId",
            table: "Orders");

        migrationBuilder.DropIndex(
            name: "IX_Orders_ProductId",
            table: "Orders");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Orders_CustomerId",
            table: "Orders",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_ProductId",
            table: "Orders",
            column: "ProductId");

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_Customers_CustomerId",
            table: "Orders",
            column: "CustomerId",
            principalTable: "Customers",
            principalColumn: "CustomerId",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_Products_ProductId",
            table: "Orders",
            column: "ProductId",
            principalTable: "Products",
            principalColumn: "ProductId",
            onDelete: ReferentialAction.Cascade);
    }
}
