using System.ComponentModel.DataAnnotations;

namespace SistemaControleEstoque.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Informe o nome do produto")]
        public string? Nome { get; set; }
        [Required(ErrorMessage ="Informe e descrição do produto")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage ="Informe a quantidade do produto")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public float? Quantidade { get; set; }

        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }


    }
}
