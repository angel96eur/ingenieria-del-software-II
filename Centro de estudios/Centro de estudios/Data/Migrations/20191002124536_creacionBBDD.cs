using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Centro_de_estudios.Data.Migrations
{
    public partial class creacionBBDD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimerApellido",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SegundoApellido",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Impartir",
                columns: table => new
                {
                    ImpartirId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfesorId = table.Column<string>(nullable: true),
                    mesesDocenciaTotal = table.Column<int>(nullable: false),
                    fecha = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impartir", x => x.ImpartirId);
                    table.ForeignKey(
                        name: "FK_Impartir_AspNetUsers_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Intensificacion",
                columns: table => new
                {
                    IntensificacionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreIntensificacion = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intensificacion", x => x.IntensificacionID);
                });

            migrationBuilder.CreateTable(
                name: "MetodoPago",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(nullable: false),
                    CreditCardNumber = table.Column<string>(nullable: true),
                    CCV = table.Column<string>(nullable: true),
                    ExpìrationDate = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Prefix = table.Column<string>(maxLength: 3, nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodoPago", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipoMaterial",
                columns: table => new
                {
                    TipoMaterialID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMaterial", x => x.TipoMaterialID);
                });

            migrationBuilder.CreateTable(
                name: "Asignatura",
                columns: table => new
                {
                    AsignaturaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreAsignatura = table.Column<string>(maxLength: 35, nullable: false),
                    Precio = table.Column<int>(nullable: false),
                    FechaComienzo = table.Column<DateTime>(nullable: false),
                    MinimoMesesDocencia = table.Column<int>(nullable: false),
                    IntensificacionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignatura", x => x.AsignaturaID);
                    table.ForeignKey(
                        name: "FK_Asignatura_Intensificacion_IntensificacionID",
                        column: x => x.IntensificacionID,
                        principalTable: "Intensificacion",
                        principalColumn: "IntensificacionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    CompraID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstudianteID = table.Column<string>(nullable: true),
                    PrecioTotal = table.Column<double>(nullable: false),
                    FechaCompra = table.Column<DateTime>(nullable: false),
                    DeireccionDeEnvio = table.Column<string>(nullable: false),
                    MetodoDePagoID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.CompraID);
                    table.ForeignKey(
                        name: "FK_Compra_AspNetUsers_EstudianteID",
                        column: x => x.EstudianteID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_MetodoPago_MetodoDePagoID",
                        column: x => x.MetodoDePagoID,
                        principalTable: "MetodoPago",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matricula",
                columns: table => new
                {
                    MatriculaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstudianteId = table.Column<string>(nullable: true),
                    PrecioTotal = table.Column<double>(nullable: false),
                    FechaMatricula = table.Column<DateTime>(nullable: false),
                    Residencia = table.Column<string>(nullable: false),
                    MetodoPagoID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matricula", x => x.MatriculaId);
                    table.ForeignKey(
                        name: "FK_Matricula_AspNetUsers_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matricula_MetodoPago_MetodoPagoID",
                        column: x => x.MetodoPagoID,
                        principalTable: "MetodoPago",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    MaterialID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(maxLength: 70, nullable: false),
                    PrecioCompra = table.Column<int>(nullable: false),
                    FechaLanzamiento = table.Column<DateTime>(nullable: false),
                    TipoMaterialID = table.Column<int>(nullable: false),
                    CantidadCompra = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.MaterialID);
                    table.ForeignKey(
                        name: "FK_Material_TipoMaterial_TipoMaterialID",
                        column: x => x.TipoMaterialID,
                        principalTable: "TipoMaterial",
                        principalColumn: "TipoMaterialID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "impartirAsignatura",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidadAsignatura = table.Column<int>(nullable: false),
                    AsignaturaId = table.Column<int>(nullable: false),
                    ImpartirId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_impartirAsignatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_impartirAsignatura_Asignatura_AsignaturaId",
                        column: x => x.AsignaturaId,
                        principalTable: "Asignatura",
                        principalColumn: "AsignaturaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_impartirAsignatura_Impartir_ImpartirId",
                        column: x => x.ImpartirId,
                        principalTable: "Impartir",
                        principalColumn: "ImpartirId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matricula_Asignatura",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MesesDocencia = table.Column<int>(nullable: false),
                    AsignaturaId = table.Column<int>(nullable: false),
                    MatriculaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matricula_Asignatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matricula_Asignatura_Asignatura_AsignaturaId",
                        column: x => x.AsignaturaId,
                        principalTable: "Asignatura",
                        principalColumn: "AsignaturaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matricula_Asignatura_Matricula_MatriculaId",
                        column: x => x.MatriculaId,
                        principalTable: "Matricula",
                        principalColumn: "MatriculaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompraMaterial",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(nullable: false),
                    MaterialID = table.Column<int>(nullable: false),
                    CompraID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraMaterial", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CompraMaterial_Compra_CompraID",
                        column: x => x.CompraID,
                        principalTable: "Compra",
                        principalColumn: "CompraID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompraMaterial_Material_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Material",
                        principalColumn: "MaterialID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asignatura_IntensificacionID",
                table: "Asignatura",
                column: "IntensificacionID");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_EstudianteID",
                table: "Compra",
                column: "EstudianteID");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_MetodoDePagoID",
                table: "Compra",
                column: "MetodoDePagoID");

            migrationBuilder.CreateIndex(
                name: "IX_CompraMaterial_CompraID",
                table: "CompraMaterial",
                column: "CompraID");

            migrationBuilder.CreateIndex(
                name: "IX_CompraMaterial_MaterialID",
                table: "CompraMaterial",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_Impartir_ProfesorId",
                table: "Impartir",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_impartirAsignatura_AsignaturaId",
                table: "impartirAsignatura",
                column: "AsignaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_impartirAsignatura_ImpartirId",
                table: "impartirAsignatura",
                column: "ImpartirId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_TipoMaterialID",
                table: "Material",
                column: "TipoMaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_Matricula_EstudianteId",
                table: "Matricula",
                column: "EstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_Matricula_MetodoPagoID",
                table: "Matricula",
                column: "MetodoPagoID");

            migrationBuilder.CreateIndex(
                name: "IX_Matricula_Asignatura_AsignaturaId",
                table: "Matricula_Asignatura",
                column: "AsignaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Matricula_Asignatura_MatriculaId",
                table: "Matricula_Asignatura",
                column: "MatriculaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompraMaterial");

            migrationBuilder.DropTable(
                name: "impartirAsignatura");

            migrationBuilder.DropTable(
                name: "Matricula_Asignatura");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Impartir");

            migrationBuilder.DropTable(
                name: "Asignatura");

            migrationBuilder.DropTable(
                name: "Matricula");

            migrationBuilder.DropTable(
                name: "TipoMaterial");

            migrationBuilder.DropTable(
                name: "Intensificacion");

            migrationBuilder.DropTable(
                name: "MetodoPago");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimerApellido",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SegundoApellido",
                table: "AspNetUsers");
        }
    }
}
