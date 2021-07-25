using Microsoft.EntityFrameworkCore.Migrations;

namespace Centro_de_estudios.Data.Migrations
{
    public partial class correcionCU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cantidad",
                table: "Matricula_Asignatura",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "cantidadAlumnos",
                table: "Asignatura",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cantidad",
                table: "Matricula_Asignatura");

            migrationBuilder.DropColumn(
                name: "cantidadAlumnos",
                table: "Asignatura");
        }
    }
}
