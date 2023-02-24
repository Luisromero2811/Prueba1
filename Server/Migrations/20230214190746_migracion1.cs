using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prueba1.Server.Migrations
{
    /// <inheritdoc />
    public partial class migracion1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculasId",
                table: "GenerosPeliculas");

            migrationBuilder.DropForeignKey(
                name: "FK_PeliculasActores_Peliculas_PeliculasId",
                table: "PeliculasActores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculasActores",
                table: "PeliculasActores");

            migrationBuilder.DropIndex(
                name: "IX_PeliculasActores_PeliculasId",
                table: "PeliculasActores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas");

            migrationBuilder.DropColumn(
                name: "PeliculaId",
                table: "PeliculasActores");

            migrationBuilder.DropColumn(
                name: "PeliculaId",
                table: "GenerosPeliculas");

            migrationBuilder.AlterColumn<int>(
                name: "PeliculasId",
                table: "PeliculasActores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PeliculasId",
                table: "GenerosPeliculas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculasActores",
                table: "PeliculasActores",
                columns: new[] { "PeliculasId", "ActorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas",
                columns: new[] { "GeneroId", "PeliculasId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculasId",
                table: "GenerosPeliculas",
                column: "PeliculasId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculasActores_Peliculas_PeliculasId",
                table: "PeliculasActores",
                column: "PeliculasId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculasId",
                table: "GenerosPeliculas");

            migrationBuilder.DropForeignKey(
                name: "FK_PeliculasActores_Peliculas_PeliculasId",
                table: "PeliculasActores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculasActores",
                table: "PeliculasActores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas");

            migrationBuilder.AlterColumn<int>(
                name: "PeliculasId",
                table: "PeliculasActores",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PeliculaId",
                table: "PeliculasActores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PeliculasId",
                table: "GenerosPeliculas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PeliculaId",
                table: "GenerosPeliculas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculasActores",
                table: "PeliculasActores",
                columns: new[] { "PeliculaId", "ActorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas",
                columns: new[] { "GeneroId", "PeliculaId" });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasActores_PeliculasId",
                table: "PeliculasActores",
                column: "PeliculasId");

            migrationBuilder.AddForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculasId",
                table: "GenerosPeliculas",
                column: "PeliculasId",
                principalTable: "Peliculas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculasActores_Peliculas_PeliculasId",
                table: "PeliculasActores",
                column: "PeliculasId",
                principalTable: "Peliculas",
                principalColumn: "Id");
        }
    }
}
