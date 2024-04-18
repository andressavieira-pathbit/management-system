using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.System.Infrastructure.Migrations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class UpdateGuid : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Users",
            type: "uuid",
            nullable: false,
            defaultValueSql: "uuid_generate_v4()",
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Products",
            type: "uuid",
            nullable: false,
            defaultValueSql: "uuid_generate_v4()",
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Orders",
            type: "uuid",
            nullable: false,
            defaultValueSql: "uuid_generate_v4()",
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Customers",
            type: "uuid",
            nullable: false,
            defaultValueSql: "uuid_generate_v4()",
            oldClrType: typeof(Guid),
            oldType: "uuid");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Users",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "uuid_generate_v4()");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Products",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "uuid_generate_v4()");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Orders",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "uuid_generate_v4()");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Customers",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "uuid_generate_v4()");
    }
}
