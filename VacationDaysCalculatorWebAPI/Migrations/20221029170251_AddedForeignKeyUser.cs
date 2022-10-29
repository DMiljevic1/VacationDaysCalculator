using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationDaysCalculatorWebAPI.Migrations
{
    public partial class AddedForeignKeyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VacationDays_UserId",
                table: "VacationDays",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RemainingVacationDays_UserId",
                table: "RemainingVacationDays",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RemainingVacationDays_Users_UserId",
                table: "RemainingVacationDays",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationDays_Users_UserId",
                table: "VacationDays",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RemainingVacationDays_Users_UserId",
                table: "RemainingVacationDays");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationDays_Users_UserId",
                table: "VacationDays");

            migrationBuilder.DropIndex(
                name: "IX_VacationDays_UserId",
                table: "VacationDays");

            migrationBuilder.DropIndex(
                name: "IX_RemainingVacationDays_UserId",
                table: "RemainingVacationDays");
        }
    }
}
