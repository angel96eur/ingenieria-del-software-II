using Microsoft.EntityFrameworkCore.Migrations;

namespace Centro_de_estudios.Data.Migrations
{
    public partial class updatecuangel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estudios",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estudios",
                table: "AspNetUsers");
        }
    }
}
