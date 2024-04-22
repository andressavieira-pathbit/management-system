using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Management.System.Infrastructure.Migrations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class RemoveDoubleFk : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Customers_Users_UserId",
            table: "Customers");

        migrationBuilder.DropIndex(
            name: "IX_Customers_UserId",
            table: "Customers");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Customers_UserId",
            table: "Customers",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Customers_Users_UserId",
            table: "Customers",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "UserId",
            onDelete: ReferentialAction.Cascade);
    }
}
