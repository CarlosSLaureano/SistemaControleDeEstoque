using ControleDeContatos.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SistemaControleEstoque.Enums;
using SistemaControleEstoque.Helper;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SistemaControleEstoque.Tests.Controllers
{
    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _storage = new();
        public bool IsAvailable => true;
        public string Id => "test-session";
        public IEnumerable<string> Keys => _storage.Keys;
        public void Clear() => _storage.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _storage.Remove(key);
        public void Set(string key, byte[] value) => _storage[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _storage.TryGetValue(key, out value);
    }

    public class LoginControllerTests
    {
        private readonly Mock<IUsuarioRepositorio> _mockUsuarioRepo;
        private readonly Mock<ISessao> _mockSessao;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _mockUsuarioRepo = new Mock<IUsuarioRepositorio>();
            _mockSessao = new Mock<ISessao>();

            _controller = new LoginController(_mockUsuarioRepo.Object, _mockSessao.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            _controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void Index_QuandoUsuarioLogado_DeveRedirecionarParaHome()
        {
            // Arrange
            _mockSessao.Setup(s => s.BuscarSessaoDoUsuario()).Returns(new UsuarioModel { Nome = "Carlos" });

            // Act
            var result = _controller.Index();

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }

        [Fact]
        public void Index_QuandoNaoLogado_DeveRetornarView()
        {
            // Arrange
            _mockSessao.Setup(s => s.BuscarSessaoDoUsuario()).Returns((UsuarioModel)null);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Sair_DeveRemoverSessaoERedirecionarParaLogin()
        {
            // Act
            var result = _controller.Sair();

            // Assert
            _mockSessao.Verify(s => s.RemoverSessaoUsuario(), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Login", redirect.ControllerName);
        }

        [Fact]
        public void Entrar_ComCredenciaisValidas_DeveCriarSessaoERedirecionarParaHome()
        {
            // Arrange
            var usuario = new UsuarioModel
            {
                Id = 1,
                Nome = "Admin",
                Login = "admin",
                Senha = "123".GerarHash(),
                Perfil = PerfilEnum.Administrador
            };
            _mockUsuarioRepo.Setup(r => r.BuscarPorLogin("admin")).Returns(usuario);

            var loginModel = new LoginModel { Login = "admin", Senha = "123" };

            // Act
            var result = _controller.Entrar(loginModel);

            // Assert
            _mockSessao.Verify(s => s.CriarSessaoDoUsuario(usuario), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Home", redirect.ControllerName);
        }

        [Fact]
        public void Entrar_ComSenhaInvalida_DeveDefinirMensagemErroERetornarView()
        {
            // Arrange
            var usuario = new UsuarioModel
            {
                Id = 1,
                Nome = "Admin",
                Login = "admin",
                Senha = "123".GerarHash()
            };
            _mockUsuarioRepo.Setup(r => r.BuscarPorLogin("admin")).Returns(usuario);

            var loginModel = new LoginModel { Login = "admin", Senha = "senhaErrada" };

            // Act
            var result = _controller.Entrar(loginModel);

            // Assert
            _mockSessao.Verify(s => s.CriarSessaoDoUsuario(It.IsAny<UsuarioModel>()), Times.Never);
            Assert.Equal("Usuário e/ou senha inválido(s). Por favor, tente novamente.", _controller.TempData["MensagemErro"]);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);
        }
    }
}
