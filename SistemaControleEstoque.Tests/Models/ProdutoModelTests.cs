using SistemaControleEstoque.Models;
using Xunit;

namespace SistemaControleEstoque.Tests.Models
{
    public class ProdutoModelTests
    {
        [Fact]
        public void ProdutoModel_DevePermitirAssociacaoComCategoria()
        {
            // Arrange
            var categoria = new CategoriaModel { Id = 1, Nome = "Eletrônicos" };
            var produto = new ProdutoModel
            {
                Id = 1,
                Nome = "Notebook",
                Descricao = "Notebook Gamer",
                Preco = 4500.00m,
                Quantidade = 5,
                CategoriaId = 1,
                Categoria = categoria
            };

            // Assert
            Assert.Equal(1, produto.CategoriaId);
            Assert.NotNull(produto.Categoria);
            Assert.Equal("Eletrônicos", produto.Categoria.Nome);
        }

        [Fact]
        public void ProdutoModel_DevePermitirCategoriaNula()
        {
            // Arrange
            var produto = new ProdutoModel
            {
                Id = 2,
                Nome = "Item Sem Categoria",
                Descricao = "Descrição",
                Preco = 10.00m,
                Quantidade = 1,
                CategoriaId = null,
                Categoria = null
            };

            // Assert
            Assert.Null(produto.CategoriaId);
            Assert.Null(produto.Categoria);
        }
    }
}
