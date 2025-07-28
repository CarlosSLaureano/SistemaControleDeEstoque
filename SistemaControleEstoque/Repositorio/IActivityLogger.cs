using SistemaControleEstoque.Models;
using System.Threading.Tasks;

namespace SistemaControleEstoque.Repositorio
{
    public interface IActivityLogger
    {
        Task LogAsync(string userName, string action, string controller, string description, int? quantidade = null);
    }
}
