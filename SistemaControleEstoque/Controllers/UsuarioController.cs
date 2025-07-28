using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SistemaControleEstoque.Filters;
using SistemaControleEstoque.Helper;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;

namespace SistemaControleEstoque.Controllers
{
    [PaginaRestritaSomenteAdmin]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();
            return View(usuarios);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _usuarioRepositorio.Apagar(id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Usuário apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, erro na deleção do usuário!";
                }

                return RedirectToAction("Index");
            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, erro na deleção do usuário, tente novamente, detalhe do erro: {erro.Message}!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    usuario = _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, erro no cadastro do usuário, tente novamente, detalhe do erro: {erro.Message}!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Editar(UsuarioSemSenhaModel usuarioSemSenhaModel)
        {
            try
            {
                UsuarioModel usuario = null;

                if (ModelState.IsValid)
                {
                    usuario = new UsuarioModel()
                    {
                        Id = usuarioSemSenhaModel.Id,
                        Nome = usuarioSemSenhaModel.Nome,
                        Login = usuarioSemSenhaModel.Login,
                        Email = usuarioSemSenhaModel.Email,
                        Perfil = usuarioSemSenhaModel.Perfil,
                    };

                    usuario = _usuarioRepositorio.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Usuário alterado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View("Editar", usuario);
            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, erro na atualização do usuário, tente novamente, detalhe do erro: {erro.Message}!";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult AlterarSenha(int id)
        {
            // Verifica se é administrador
            if (HttpContext.Session.GetString("TipoUsuario") != "Administrador")
                return RedirectToAction("Index", "Home");

            var usuario = _usuarioRepositorio.ListarPorId(id);
            if (usuario == null) return NotFound();

            return View(usuario); // Envia o usuário para a view AlterarSenha.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AlterarSenha(int id, string novaSenha)
        {
            if (HttpContext.Session.GetString("TipoUsuario") != "Administrador")
                return RedirectToAction("Index", "Home");

            var usuario = _usuarioRepositorio.ListarPorId(id);
            if (usuario == null) return NotFound();

            usuario.Senha = Criptografia.GerarHash(novaSenha); // aplica o hash na senha
            _usuarioRepositorio.Atualizar(usuario);

            TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
            return RedirectToAction("Index");
        }



        // --- Novo método para exportar usuários para Excel ---
        public IActionResult ExportarParaExcel()
        {
            var usuarios = _usuarioRepositorio.BuscarTodos();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Usuários");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nome";
                worksheet.Cell(1, 3).Value = "Login";
                worksheet.Cell(1, 4).Value = "Email";
                worksheet.Cell(1, 5).Value = "Perfil";
                worksheet.Cell(1, 6).Value = "Data de Cadastro";

                int linha = 2;
                foreach (var u in usuarios)
                {
                    worksheet.Cell(linha, 1).Value = u.Id;
                    worksheet.Cell(linha, 2).Value = u.Nome;
                    worksheet.Cell(linha, 3).Value = u.Login;
                    worksheet.Cell(linha, 4).Value = u.Email;
                    worksheet.Cell(linha, 5).Value = u.Perfil?.ToString() ?? "";
                    worksheet.Cell(linha, 6).Value = u.DataCadastro?.ToString("dd/MM/yyyy") ?? "";
                    linha++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Usuarios.xlsx");
                }
            }
        }
    }
}

