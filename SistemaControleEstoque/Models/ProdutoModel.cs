using System.ComponentModel.DataAnnotations;

namespace SistemaControleEstoque.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome do produto")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Informe a descrição do produto")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Informe a quantidade do produto")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int? Quantidade { get; set; }

        [Required(ErrorMessage = "Informe o preço do produto")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal? Preco { get; set; }

        public decimal? Total { get; set; }

        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}

