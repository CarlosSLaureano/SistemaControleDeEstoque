using Microsoft.EntityFrameworkCore;
using SistemaControleEstoque.Data;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;
using Xunit;

namespace SistemaControleEstoque.Tests.Repositorios
{
    public class ProdutoRepositorioTests
    {
        private BancoContext CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<BancoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new BancoContext(options);
        }

        [Fact]
        public void Adicionar_DeveSalvarProdutoECalcularTotal()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new ProdutoRepositorio(context);

            var produto = new ProdutoModel
            {
                Nome = "Mouse Gamer",
                Descricao = "Mouse RGB",
                Preco = 150.00m,
                Quantidade = 3
            };

            // Act
            var produtoAdicionado = repositorio.Adicionar(produto);

            // Assert
            Assert.True(produtoAdicionado.Id > 0);
            Assert.Equal(450.00m, produtoAdicionado.Total);
            Assert.NotNull(produtoAdicionado.DataCadastro);

            var produtoNoBanco = context.Produtos.Find(produtoAdicionado.Id);
            Assert.NotNull(produtoNoBanco);
            Assert.Equal("Mouse Gamer", produtoNoBanco.Nome);
        }

        [Fact]
        public void BuscarTodos_DeveRetornarProdutosComCategoria()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var categoria = new CategoriaModel { Nome = "Periféricos" };
            context.Categorias.Add(categoria);
            context.SaveChanges();

            var produto = new ProdutoModel
            {
                Nome = "Teclado Mecânico",
                Descricao = "Switch Blue",
                Preco = 300.00m,
                Quantidade = 2,
                CategoriaId = categoria.Id
            };
            context.Produtos.Add(produto);
            context.SaveChanges();

            var repositorio = new ProdutoRepositorio(context);

            // Act
            var produtos = repositorio.BuscarTodos();

            // Assert
            Assert.Single(produtos);
            Assert.NotNull(produtos[0].Categoria);
            Assert.Equal("Periféricos", produtos[0].Categoria.Nome);
        }

        [Fact]
        public void Atualizar_DeveModificarDadosECalcularNovoTotal()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var produtoOriginal = new ProdutoModel
            {
                Nome = "Monitor",
                Descricao = "60Hz",
                Preco = 500.00m,
                Quantidade = 1,
                Total = 500.00m
            };
            context.Produtos.Add(produtoOriginal);
            context.SaveChanges();

            var repositorio = new ProdutoRepositorio(context);

            var produtoAtualizado = new ProdutoModel
            {
                Id = produtoOriginal.Id,
                Nome = "Monitor 144Hz",
                Descricao = "Full HD",
                Preco = 900.00m,
                Quantidade = 2
            };

            // Act
            var resultado = repositorio.Atualizar(produtoAtualizado);

            // Assert
            Assert.Equal("Monitor 144Hz", resultado.Nome);
            Assert.Equal(1800.00m, resultado.Total);
            Assert.NotNull(resultado.DataAtualizacao);
        }

        [Fact]
        public void Apagar_DeveRemoverProdutoDoBanco()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var produto = new ProdutoModel
            {
                Nome = "Headset",
                Descricao = "7.1 Surround",
                Preco = 250.00m,
                Quantidade = 1
            };
            context.Produtos.Add(produto);
            context.SaveChanges();

            var repositorio = new ProdutoRepositorio(context);

            // Act
            bool apagado = repositorio.Apagar(produto.Id);

            // Assert
            Assert.True(apagado);
            Assert.Null(context.Produtos.Find(produto.Id));
        }
    }
}
