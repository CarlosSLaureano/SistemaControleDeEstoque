namespace SistemaControleEstoque.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string UserName { get; set; } // quem fez
        public string Action { get; set; }   // o que foi feito
        public string Controller { get; set; } // onde
        public string Description { get; set; } // detalhes
        public int? Quantidade { get; set; }
        public DateTime Timestamp { get; set; } // quando
    }
}
