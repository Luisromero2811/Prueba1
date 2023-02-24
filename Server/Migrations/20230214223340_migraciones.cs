using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prueba1.Server.Migrations
{
    /// <inheritdoc />
    public partial class migraciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeliculasActores_Actores_ActoresId",
                table: "PeliculasActores");

            migrationBuilder.DropIndex(
                name: "IX_PeliculasActores_ActoresId",
                table: "PeliculasActores");

            migrationBuilder.DropColumn(
                name: "ActoresId",
                table: "PeliculasActores");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasActores_ActorId",
                table: "PeliculasActores",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculasActores_Actores_ActorId",
                table: "PeliculasActores",
                column: "ActorId",
                principalTable: "Actores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeliculasActores_Actores_ActorId",
                table: "PeliculasActores");

            migrationBuilder.DropIndex(
                name: "IX_PeliculasActores_ActorId",
                table: "PeliculasActores");

            migrationBuilder.AddColumn<int>(
                name: "ActoresId",
                table: "PeliculasActores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasActores_ActoresId",
                table: "PeliculasActores",
                column: "ActoresId");

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculasActores_Actores_ActoresId",
                table: "PeliculasActores",
                column: "ActoresId",
                principalTable: "Actores",
                principalColumn: "Id");
        }
    }
}
