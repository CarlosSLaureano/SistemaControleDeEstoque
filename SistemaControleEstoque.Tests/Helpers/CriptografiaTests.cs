using SistemaControleEstoque.Helper;
using Xunit;

namespace SistemaControleEstoque.Tests.Helpers
{
    public class CriptografiaTests
    {
        [Fact]
        public void GerarHash_DeveRetornarHashConsistente_ParaMesmoValor()
        {
            // Arrange
            string senha = "minhasenhasupersecreta";

            // Act
            string hash1 = senha.GerarHash();
            string hash2 = senha.GerarHash();

            // Assert
            Assert.NotNull(hash1);
            Assert.NotEmpty(hash1);
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GerarHash_DeveRetornarHashesDiferentes_ParaValoresDiferentes()
        {
            // Arrange
            string senha1 = "senha123";
            string senha2 = "senha456";

            // Act
            string hash1 = senha1.GerarHash();
            string hash2 = senha2.GerarHash();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
