using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prueba1.Server.Migrations
{
    /// <inheritdoc />
    public partial class RolAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Insert Into AspNetRoles(Id, Name, NormalizedName)
                                Values('3c80a8dd-fc3e-4139-bdaa-4111a39100c0', 'admin', 'ADMIN')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete AspNetRoles where Id = '3c80a8dd-fc3e-4139-bdaa-4111a39100c0'");
        }
    }
}
//Mediante la migracion vamos a crear los roles, porque de esa manera siempre el rol estará creado