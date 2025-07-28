using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SistemaControleEstoque.Filters;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;

namespace SistemaControleEstoque.Controllers
{
    //[PaginaRestritaSomenteAdmin] // Se quiser controle de acesso igual ao usuário
    public class ClienteController : Controller
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteController(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // LISTAGEM
        public IActionResult Index()
        {
            List<ClienteModel> clientes = _clienteRepositorio.BuscarTodos();
            return View(clientes);
        }

        // GET: Cliente/Criar
        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        // POST: Cliente/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(ClienteModel cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _clienteRepositorio.Adicionar(cliente);
                    TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(cliente);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar cliente: {ex.Message}";
                return View(cliente);
            }
        }

        // GET: Cliente/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var cliente = _clienteRepositorio.ListarPorId(id);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Cliente/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(ClienteModel cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _clienteRepositorio.Atualizar(cliente);
                    TempData["MensagemSucesso"] = "Cliente atualizado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(cliente);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar cliente: {ex.Message}";
                return View(cliente);
            }
        }

        // GET: Cliente/ApagarConfirmacao/5
        [HttpGet]
        public IActionResult ApagarConfirmacao(int id)
        {
            var cliente = _clienteRepositorio.ListarPorId(id);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Cliente/Apagar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _clienteRepositorio.Apagar(id);
                if (apagado)
                    TempData["MensagemSucesso"] = "Cliente apagado com sucesso!";
                else
                    TempData["MensagemErro"] = "Erro ao apagar cliente.";

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao apagar cliente: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // EXPORTAR PARA EXCEL
        public IActionResult ExportarParaExcel()
        {
            var clientes = _clienteRepositorio.BuscarTodos();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Clientes");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nome";
                worksheet.Cell(1, 3).Value = "Telefone";
                worksheet.Cell(1, 4).Value = "Data de Nascimento";
                worksheet.Cell(1, 5).Value = "Email";
                worksheet.Cell(1, 6).Value = "Data de Cadastro";

                int linha = 2;
                foreach (var c in clientes)
                {
                    worksheet.Cell(linha, 1).Value = c.Id;
                    worksheet.Cell(linha, 2).Value = c.Nome;
                    worksheet.Cell(linha, 3).Value = c.Telefone;
                    worksheet.Cell(linha, 4).Value = c.DataNascimento?.ToString("dd/MM/yyyy") ?? "";
                    worksheet.Cell(linha, 5).Value = c.Email;
                    worksheet.Cell(linha, 6).Value = c.DataCadastro?.ToString("dd/MM/yyyy") ?? "";
                    linha++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Clientes.xlsx");
                }
            }
        }
    }
}
