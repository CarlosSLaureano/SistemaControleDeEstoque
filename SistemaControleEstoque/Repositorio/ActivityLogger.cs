using Newtonsoft.Json;
using SistemaControleEstoque.Data;
using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.Repositorio
{
    public class ActivityLogger : IActivityLogger
    {
        private readonly BancoContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActivityLogger(BancoContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string userName, string action, string controller, string description, int? quantidade = null)
        {
            string sessaoUsuario = _httpContextAccessor.HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (!string.IsNullOrEmpty(sessaoUsuario))
            {
                var usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
                if (usuario != null)
                    userName = usuario.Nome; // ou usuario.Login
            }

            var log = new ActivityLog
            {
                UserName = userName,
                Action = action,
                Controller = controller,
                Description = description,
                Quantidade = quantidade,
                Timestamp = DateTime.UtcNow // armazena em UTC, converte na View
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
