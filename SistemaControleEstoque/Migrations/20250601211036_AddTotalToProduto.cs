using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaControleEstoque.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalToProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
             name: "Total",
             table: "Produtos",
             type: "decimal(18,2)",
             nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "Total",
             table: "Produtos");
        }
    }
}
