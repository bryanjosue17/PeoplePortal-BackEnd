using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeoplePortal.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddNominaFields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "nomina_type",
            table: "vouchers",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "ComprobanteDepago");

        migrationBuilder.AddColumn<string>(
            name: "notes",
            table: "vouchers",
            type: "character varying(500)",
            maxLength: 500,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "nomina_type", table: "vouchers");
        migrationBuilder.DropColumn(name: "notes",       table: "vouchers");
    }
}
