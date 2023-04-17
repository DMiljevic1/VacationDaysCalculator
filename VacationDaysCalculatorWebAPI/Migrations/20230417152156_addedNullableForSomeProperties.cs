using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationDaysCalculatorWebAPI.Migrations
{
    public partial class addedNullableForSomeProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SickLeaveTo",
                table: "SickLeave",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Attachment",
                table: "MedicalCertificates",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SickLeaveTo",
                table: "SickLeave",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Attachment",
                table: "MedicalCertificates",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }
    }
}
