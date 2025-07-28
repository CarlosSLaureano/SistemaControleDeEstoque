using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaControleEstoque.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantidadeToActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "ActivityLogs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "ActivityLogs");
        }
    }
}
