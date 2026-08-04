using Microsoft.AspNetCore.Mvc;
using SistemaControleEstoque.Filters;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;
using System.Diagnostics;
using System.Linq;

namespace SistemaControleEstoque.Controllers
{

    [PaginaParaUsuarioLogado]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;

        public HomeController(
            ILogger<HomeController> logger,
            IProdutoRepositorio produtoRepositorio,
            IClienteRepositorio clienteRepositorio)
        {
            _logger = logger;
            _produtoRepositorio = produtoRepositorio;
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index()
        {
            var produtos = _produtoRepositorio.BuscarTodos();
            var clientes = _clienteRepositorio.BuscarTodos();

            int totalProdutosEstoque = produtos.Sum(p => p.Quantidade ?? 0);
            int totalClientes = clientes.Count;
            int itensEmBaixa = produtos.Count(p => (p.Quantidade ?? 0) < 5);

            ViewBag.TotalProdutosEstoque = totalProdutosEstoque;
            ViewBag.TotalClientes = totalClientes;
            ViewBag.ItensEmBaixa = itensEmBaixa;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
