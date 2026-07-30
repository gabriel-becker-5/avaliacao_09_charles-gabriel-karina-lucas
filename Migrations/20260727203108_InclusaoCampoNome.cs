using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace avaliacao_09_charles_gabriel_karina_lucas.Migrations
{
    /// <inheritdoc />
    public partial class InclusaoCampoNome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Usuario",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Usuario");
        }
    }
}
