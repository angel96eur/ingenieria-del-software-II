using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Centro_de_estudios.Data.Migrations
{
    public partial class correccionBug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpìrationDate",
                table: "MetodoPago");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "MetodoPago",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "MetodoPago",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "MetodoPago");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "MetodoPago",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 7,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpìrationDate",
                table: "MetodoPago",
                type: "datetime2",
                nullable: true);
        }
    }
}
