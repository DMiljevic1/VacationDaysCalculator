using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationDaysCalculatorWebAPI.Migrations
{
    public partial class addedEnumSickLeaveStatusInTableSickLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "SickLeave");

            migrationBuilder.AddColumn<int>(
                name: "SickLeaveStatus",
                table: "SickLeave",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SickLeaveStatus",
                table: "SickLeave");

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "SickLeave",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
