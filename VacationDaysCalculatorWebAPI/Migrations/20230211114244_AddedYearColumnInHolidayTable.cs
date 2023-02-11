using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationDaysCalculatorWebAPI.Migrations
{
    public partial class AddedYearColumnInHolidayTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Holidays");
        }
    }
}
