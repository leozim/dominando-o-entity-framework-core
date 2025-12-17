using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DominandoEFCore.Migrations
{
    /// <inheritdoc />
    public partial class FuncionarioRg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rg",
                table: "Funcionarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rg",
                table: "Funcionarios");
        }
    }
}
