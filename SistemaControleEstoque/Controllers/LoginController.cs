using Microsoft.AspNetCore.Mvc;
using SistemaControleEstoque.Helper;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;

        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoDoUsuario() != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult RedefinirSenha()
        {
            ViewBag.Mensagem = "Para redefinir sua senha, entre em contato com o administrador do sistema.";
            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if (usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoDoUsuario(usuario);

                            // Ajuste aqui para gravar "Administrador" ou "Usuario"
                            string tipoUsuario = usuario.Perfil == SistemaControleEstoque.Enums.PerfilEnum.Administrador
                                ? "Administrador"
                                : "Usuario";

                            HttpContext.Session.SetString("TipoUsuario", tipoUsuario);
                            HttpContext.Session.SetString("UserName", usuario.Nome);

                            return RedirectToAction("Index", "Home");
                        }

                        TempData["MensagemErro"] = "Usuário e/ou senha inválido(s). Por favor, tente novamente.";
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Usuário e/ou senha inválido(s). Por favor, tente novamente.";
                    }
                }

                return View("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos realizar seu login, tente novamente. Detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
