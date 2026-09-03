using Microsoft.EntityFrameworkCore;
using SistemaControleEstoque.Data;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;
using Xunit;

namespace SistemaControleEstoque.Tests.Repositorios
{
    public class CategoriaRepositorioTests
    {
        private BancoContext CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<BancoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new BancoContext(options);
        }

        [Fact]
        public void Adicionar_DeveSalvarCategoriaComDataCadastro()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var repositorio = new CategoriaRepositorio(context);
            var categoria = new CategoriaModel { Nome = "Informática" };

            // Act
            var categoriaSalva = repositorio.Adicionar(categoria);

            // Assert
            Assert.True(categoriaSalva.Id > 0);
            Assert.NotNull(categoriaSalva.DataCadastro);
            Assert.Equal("Informática", categoriaSalva.Nome);
        }

        [Fact]
        public void Atualizar_DeveModificarNomeComDataAtualizacao()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var categoria = new CategoriaModel { Nome = "Papelaria" };
            context.Categorias.Add(categoria);
            context.SaveChanges();

            var repositorio = new CategoriaRepositorio(context);
            var categoriaAtualizada = new CategoriaModel { Id = categoria.Id, Nome = "Papelaria & Escritório" };

            // Act
            var resultado = repositorio.Atualizar(categoriaAtualizada);

            // Assert
            Assert.Equal("Papelaria & Escritório", resultado.Nome);
            Assert.NotNull(resultado.DataAtualizacao);
        }

        [Fact]
        public void Apagar_DeveRemoverCategoria()
        {
            // Arrange
            using var context = CriarContextoEmMemoria();
            var categoria = new CategoriaModel { Nome = "Livros" };
            context.Categorias.Add(categoria);
            context.SaveChanges();

            var repositorio = new CategoriaRepositorio(context);

            // Act
            bool apagado = repositorio.Apagar(categoria.Id);

            // Assert
            Assert.True(apagado);
            Assert.Null(context.Categorias.Find(categoria.Id));
        }
    }
}
