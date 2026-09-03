using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SistemaControleEstoque.Controllers;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;
using System.Collections.Generic;
using Xunit;

namespace SistemaControleEstoque.Tests.Controllers
{
    public class CategoriaControllerTests
    {
        private readonly Mock<ICategoriaRepositorio> _mockCategoriaRepo;
        private readonly CategoriaController _controller;

        public CategoriaControllerTests()
        {
            _mockCategoriaRepo = new Mock<ICategoriaRepositorio>();
            _controller = new CategoriaController(_mockCategoriaRepo.Object);

            var httpContext = new DefaultHttpContext();
            _controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void Index_DeveRetornarViewComTodasCategorias()
        {
            // Arrange
            var categorias = new List<CategoriaModel>
            {
                new CategoriaModel { Id = 1, Nome = "Bebidas" },
                new CategoriaModel { Id = 2, Nome = "Alimentos" }
            };
            _mockCategoriaRepo.Setup(r => r.BuscarTodos()).Returns(categorias);

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<CategoriaModel>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Criar_POST_QuandoValido_DeveAdicionarERedirecionar()
        {
            // Arrange
            var categoria = new CategoriaModel { Nome = "Limpeza" };

            // Act
            var result = _controller.Criar(categoria);

            // Assert
            _mockCategoriaRepo.Verify(r => r.Adicionar(categoria), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Categoria cadastrada com sucesso!", _controller.TempData["MensagemSucesso"]);
        }

        [Fact]
        public void Apagar_QuandoSucesso_DeveRedirecionarComMensagemSucesso()
        {
            // Arrange
            _mockCategoriaRepo.Setup(r => r.Apagar(1)).Returns(true);

            // Act
            var result = _controller.Apagar(1);

            // Assert
            _mockCategoriaRepo.Verify(r => r.Apagar(1), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Categoria apagada com sucesso!", _controller.TempData["MensagemSucesso"]);
        }
    }
}
