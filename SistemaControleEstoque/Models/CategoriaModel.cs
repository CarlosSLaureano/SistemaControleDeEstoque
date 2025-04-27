using System.ComponentModel.DataAnnotations;

namespace SistemaControleEstoque.Models
{
    public class CategoriaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o nome da categoria")]
        public string? Nome { get; set; }

        public DateTime? DataCadastro { get; set; }

        public DateTime? DataAtualizacao { get; set; }

       
        
    }
}
