using Microsoft.AspNetCore.Mvc;
using SistemaControleEstoque.Filters;

namespace SistemaControleEstoque.Controllers
{
    [PaginaParaUsuarioLogado]
    public class RestritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
