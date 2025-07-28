using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Busca o usuário logado da sessão
            string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario))
                return Content(""); // Se não houver sessão, não renderiza nada

            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);

            return View(usuario); // Envia o usuário para a view
        }
    }
}
