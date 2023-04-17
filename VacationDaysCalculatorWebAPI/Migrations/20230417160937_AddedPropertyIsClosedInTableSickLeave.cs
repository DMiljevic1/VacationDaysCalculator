using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationDaysCalculatorWebAPI.Migrations
{
    public partial class AddedPropertyIsClosedInTableSickLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "SickLeave",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "SickLeave");
        }
    }
}
