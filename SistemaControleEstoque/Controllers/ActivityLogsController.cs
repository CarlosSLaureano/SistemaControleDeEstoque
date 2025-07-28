using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SistemaControleEstoque.Data;
using SistemaControleEstoque.Filters;

namespace SistemaControleEstoque.Controllers
{
    [PaginaParaUsuarioLogado]
    public class ActivityLogsController : Controller
    {
        private readonly BancoContext _context;

        public ActivityLogsController(BancoContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime? dataInicio, DateTime? dataFim)
        {
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (!string.Equals(tipoUsuario, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensagemErro"] = "Acesso restrito a administradores!";
                return RedirectToAction("Index", "Home");
            }

            var query = _context.ActivityLogs.AsQueryable();

            if (dataInicio.HasValue)
                query = query.Where(l => l.Timestamp >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(l => l.Timestamp <= dataFim.Value.AddDays(1).AddSeconds(-1));

            var logs = query
                .OrderByDescending(l => l.Timestamp)
                .Take(500)
                .ToList();

            return View(logs);
        }


        public IActionResult ExportarParaExcel(DateTime dataInicio, DateTime dataFim)
        {
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            if (tipoUsuario?.ToLower() != "Administrador")
            {
                TempData["MensagemErro"] = "Acesso restrito a administradores!";
                return RedirectToAction("Index", "Home");
            }

            var logs = _context.ActivityLogs
                .Where(l => l.Timestamp >= dataInicio && l.Timestamp <= dataFim.AddDays(1).AddSeconds(-1))
                .OrderByDescending(l => l.Timestamp)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Logs de Atividade");

                // Cabeçalhos
                worksheet.Cell(1, 1).Value = "Usuário";
                worksheet.Cell(1, 2).Value = "Ação";
                worksheet.Cell(1, 3).Value = "Controller";
                worksheet.Cell(1, 4).Value = "Descrição";
                worksheet.Cell(1, 5).Value = "Data/Hora";

                // Dados
                for (int i = 0; i < logs.Count; i++)
                {
                    var log = logs[i];
                    worksheet.Cell(i + 2, 1).Value = log.UserName;
                    worksheet.Cell(i + 2, 2).Value = log.Action;
                    worksheet.Cell(i + 2, 3).Value = log.Controller;
                    worksheet.Cell(i + 2, 4).Value = log.Description;
                    worksheet.Cell(i + 2, 5).Value = log.Timestamp.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                }

                // Formata e exporta
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Logs_Atividades_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                }
            }
        }
    }
}
