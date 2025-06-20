using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foro_C.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPropiedadesEntrada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "Entradas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Destacada",
                table: "Entradas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Entradas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Imagen",
                table: "Entradas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resumen",
                table: "Entradas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "Activa",
                table: "Entradas");

            migrationBuilder.DropColumn(
                name: "Destacada",
                table: "Entradas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Entradas");

            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Entradas");

            migrationBuilder.DropColumn(
                name: "Resumen",
                table: "Entradas");
        }
    }
}
