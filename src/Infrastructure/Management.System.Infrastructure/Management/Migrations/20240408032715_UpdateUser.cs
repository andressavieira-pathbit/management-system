using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.System.Infrastructure.Migrations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class UpdateUser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Customers_CustomerId",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_CustomerId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "CustomerId",
            table: "Users");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
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
            name: "FK_Users_Customers_CustomerId",
            table: "Users",
            column: "CustomerId",
            principalTable: "Customers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
