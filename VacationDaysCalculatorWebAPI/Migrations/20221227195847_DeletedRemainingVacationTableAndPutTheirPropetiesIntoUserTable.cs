using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationDaysCalculatorWebAPI.Migrations
{
    public partial class DeletedRemainingVacationTableAndPutTheirPropetiesIntoUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemainingVacation");

            migrationBuilder.AddColumn<int>(
                name: "CurrentYear",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RemainingDaysOffCurrentYear",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RemainingDaysOffLastYear",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentYear",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RemainingDaysOffCurrentYear",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RemainingDaysOffLastYear",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "RemainingVacation",
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
                    table.PrimaryKey("PK_RemainingVacation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RemainingVacation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemainingVacation_UserId",
                table: "RemainingVacation",
                column: "UserId");
        }
    }
}
