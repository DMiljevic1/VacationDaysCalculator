using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationDaysCalculatorWebAPI.Migrations
{
    public partial class AddedFileSizeAndFileNameColumnsInMedCertTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "MedicalCertificates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "MedicalCertificates",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "MedicalCertificates");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "MedicalCertificates");
        }
    }
}
