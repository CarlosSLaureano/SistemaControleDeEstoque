using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SistemaControleEstoque.Controllers;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SistemaControleEstoque.Tests.Controllers
{
    public class ProdutoControllerTests
    {
        private readonly Mock<IProdutoRepositorio> _mockProdutoRepo;
        private readonly Mock<ICategoriaRepositorio> _mockCategoriaRepo;
        private readonly Mock<IActivityLogger> _mockLogger;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly ProdutoController _controller;

        public ProdutoControllerTests()
        {
            _mockProdutoRepo = new Mock<IProdutoRepositorio>();
            _mockCategoriaRepo = new Mock<ICategoriaRepositorio>();
            _mockLogger = new Mock<IActivityLogger>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            _controller = new ProdutoController(
                _mockProdutoRepo.Object,
                _mockCategoriaRepo.Object,
                _mockLogger.Object,
                _mockHttpContextAccessor.Object
            );

            var httpContext = new DefaultHttpContext();
            _controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void Index_DeveRetornarViewComListaDeProdutos()
        {
            // Arrange
            var produtos = new List<ProdutoModel>
            {
                new ProdutoModel { Id = 1, Nome = "Produto 1" },
                new ProdutoModel { Id = 2, Nome = "Produto 2" }
            };
            _mockProdutoRepo.Setup(r => r.BuscarTodos()).Returns(produtos);

            // Act
            var result = _controller.Index(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ProdutoModel>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Criar_GET_DeveRetornarViewComCategoriasEmViewBag()
        {
            // Arrange
            var categorias = new List<CategoriaModel>
            {
                new CategoriaModel { Id = 1, Nome = "Cat A" },
                new CategoriaModel { Id = 2, Nome = "Cat B" }
            };
            _mockCategoriaRepo.Setup(r => r.BuscarTodos()).Returns(categorias);

            // Act
            var result = _controller.Criar();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(_controller.ViewBag.Categorias);
        }

        [Fact]
        public async Task Criar_POST_QuandoValido_DeveAdicionarERedirecionarParaIndex()
        {
            // Arrange
            var novoProduto = new ProdutoModel
            {
                Nome = "Novo Produto",
                Descricao = "Descrição",
                Preco = 100m,
                Quantidade = 2
            };

            // Act
            var result = await _controller.Criar(novoProduto);

            // Assert
            _mockProdutoRepo.Verify(r => r.Adicionar(novoProduto), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Produto cadastrado com sucesso!", _controller.TempData["MensagemSucesso"]);
        }

        [Fact]
        public async Task Criar_POST_QuandoInvalido_DeveRetornarMesmaView()
        {
            // Arrange
            var produtoInvalido = new ProdutoModel();
            _controller.ModelState.AddModelError("Nome", "O nome é obrigatório");

            // Act
            var result = await _controller.Criar(produtoInvalido);

            // Assert
            _mockProdutoRepo.Verify(r => r.Adicionar(It.IsAny<ProdutoModel>()), Times.Never);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(produtoInvalido, viewResult.Model);
        }
    }
}
