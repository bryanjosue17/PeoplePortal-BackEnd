using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeoplePortal.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hr_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    employee_id = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    vacation_start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    vacation_end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    certificate_type = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    hr_comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    reviewed_by = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hr_requests", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_hr_requests_employee_id",
                table: "hr_requests",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_hr_requests_status",
                table: "hr_requests",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hr_requests");
        }
    }
}
