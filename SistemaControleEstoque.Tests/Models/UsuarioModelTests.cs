using SistemaControleEstoque.Helper;
using SistemaControleEstoque.Models;
using Xunit;

namespace SistemaControleEstoque.Tests.Models
{
    public class UsuarioModelTests
    {
        [Fact]
        public void SenhaValida_DeveRetornarTrue_QuandoSenhaEstiverCorreta()
        {
            // Arrange
            var usuario = new UsuarioModel
            {
                Nome = "Carlos",
                Login = "carlos",
                Senha = "123".GerarHash()
            };

            // Act
            bool resultado = usuario.SenhaValida("123");

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void SenhaValida_DeveRetornarFalse_QuandoSenhaEstiverIncorreta()
        {
            // Arrange
            var usuario = new UsuarioModel
            {
                Nome = "Carlos",
                Login = "carlos",
                Senha = "123".GerarHash()
            };

            // Act
            bool resultado = usuario.SenhaValida("senhaErrada");

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void SetSenhaHash_DeveHashearSenha()
        {
            // Arrange
            var usuario = new UsuarioModel
            {
                Senha = "minhaSenha"
            };

            // Act
            usuario.SetSenhaHash();

            // Assert
            Assert.Equal("minhaSenha".GerarHash(), usuario.Senha);
        }

        [Fact]
        public void SetNovaSenha_DeveAtribuirNovaSenhaComHash()
        {
            // Arrange
            var usuario = new UsuarioModel();

            // Act
            usuario.SetNovaSenha("novaSenha123");

            // Assert
            Assert.Equal("novaSenha123".GerarHash(), usuario.Senha);
        }

        [Fact]
        public void GerarNovaSenha_DeveRetornarSenhaTextoClaro_EGuardarHash()
        {
            // Arrange
            var usuario = new UsuarioModel();

            // Act
            string senhaGerada = usuario.GerarNovaSenha();

            // Assert
            Assert.NotNull(senhaGerada);
            Assert.Equal(8, senhaGerada.Length);
            Assert.Equal(senhaGerada.GerarHash(), usuario.Senha);
        }
    }
}
