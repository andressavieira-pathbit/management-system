using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Management.System.Infrastructure.Migrations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class UpdateId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Id",
            table: "Users",
            newName: "UserId");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "Products",
            newName: "ProductId");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "Orders",
            newName: "OrderId");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "Customers",
            newName: "CustomerId");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Customers",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(100)",
            oldMaxLength: 100);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "UserId",
            table: "Users",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "ProductId",
            table: "Products",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "OrderId",
            table: "Orders",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "CustomerId",
            table: "Customers",
            newName: "Id");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Customers",
            type: "character varying(100)",
            maxLength: 100,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");
    }
}
