using Microsoft.AspNetCore.Mvc;
using SistemaControleEstoque.Filters;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaControleEstoque.Controllers
{
    [PaginaParaUsuarioLogado]

    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        public CategoriaController(ICategoriaRepositorio categoriaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }
        public IActionResult Index()
        {
            List<CategoriaModel> categorias = _categoriaRepositorio.BuscarTodos();

            return View(categorias);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {   
            CategoriaModel categoria = _categoriaRepositorio.ListarPorId(id);
            return View(categoria);
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            CategoriaModel categoria = _categoriaRepositorio.ListarPorId(id);
            return View(categoria);
            
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _categoriaRepositorio.Apagar(id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Categoria apagada com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, erro na deleção da categoria!";
                }

                
                return RedirectToAction("Index");
            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, erro na deleção da categoria, tente novamente, detalhe do erro: {erro.Message}!";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult Criar(CategoriaModel categoria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    categoria = _categoriaRepositorio.Adicionar(categoria);
                    TempData["MensagemSucesso"] = "Categoria cadastrada com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(categoria);
            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, erro no cadastro da categoria, tente novamente, detalhe do erro: {erro.Message}!";
                return RedirectToAction("Index");
            }
           
        }

        [HttpPost]
        public IActionResult Alterar(CategoriaModel categoria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _categoriaRepositorio.Atualizar(categoria);
                    TempData["MensagemSucesso"] = "Categoria alterada com sucesso!";
                    return RedirectToAction("Index");
                }
                return View("Editar", categoria);
            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, erro na atualização da categoria, tente novamente, detalhe do erro: {erro.Message}!";
                return RedirectToAction("Index");
            }
        }



    }
}
