using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationDaysCalculatorWebAPI.Migrations
{
    public partial class ChangedNamesOfTableVacationAndRemainingVacation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemainingVacationDays");

            migrationBuilder.DropTable(
                name: "VacationDays");

            migrationBuilder.CreateTable(
                name: "RemainingVacation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentYear = table.Column<int>(type: "int", nullable: false),
                    RemainingDaysOffCurrentYear = table.Column<int>(type: "int", nullable: false),
                    RemainingDaysOffLastYear = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemainingVacation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RemainingVacation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vacation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VacationFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VacationTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemainingVacation_UserId",
                table: "RemainingVacation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_UserId",
                table: "Vacation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemainingVacation");

            migrationBuilder.DropTable(
                name: "Vacation");

            migrationBuilder.CreateTable(
                name: "RemainingVacationDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CurrentYear = table.Column<int>(type: "int", nullable: false),
                    RemainingDaysOffCurrentYear = table.Column<int>(type: "int", nullable: false),
                    RemainingDaysOffLastYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemainingVacationDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RemainingVacationDays_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VacationFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VacationTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationDays_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemainingVacationDays_UserId",
                table: "RemainingVacationDays",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationDays_UserId",
                table: "VacationDays",
                column: "UserId");
        }
    }
}
