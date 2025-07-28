using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SistemaControleEstoque.Models;
using SistemaControleEstoque.Repositorio;
using System.Threading.Tasks;

namespace SistemaControleEstoque.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IActivityLogger _activityLogger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProdutoController(
            IProdutoRepositorio produtoRepositorio,
            IActivityLogger activityLogger,
            IHttpContextAccessor httpContextAccessor)
        {
            _produtoRepositorio = produtoRepositorio;
            _activityLogger = activityLogger;
            _httpContextAccessor = httpContextAccessor;
        }

        // LISTAGEM COM FILTRO POR DATA
        public IActionResult Index(DateTime? dataInicio, DateTime? dataFim)
        {
            var produtos = _produtoRepositorio.BuscarTodos();

            if (dataInicio.HasValue)
                produtos = produtos
                    .FindAll(p => p.DataCadastro.HasValue && p.DataCadastro.Value.Date >= dataInicio.Value.Date);

            if (dataFim.HasValue)
                produtos = produtos
                    .FindAll(p => p.DataCadastro.HasValue && p.DataCadastro.Value.Date <= dataFim.Value.Date);

            ViewBag.DataInicio = dataInicio?.ToString("yyyy-MM-dd");
            ViewBag.DataFim = dataFim?.ToString("yyyy-MM-dd");

            return View(produtos);
        }

        // TELA DE CADASTRO DE PRODUTO
        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        // RECEBE O POST DO FORMULÁRIO DE PRODUTO
        [HttpPost]
        public async Task<IActionResult> Criar(ProdutoModel produto)
        {
            if (ModelState.IsValid)
            {
                _produtoRepositorio.Adicionar(produto);

                await RegistrarLogAsync("Criar", "Produto", $"Produto criado: {produto.Nome}", produto.Quantidade);

                TempData["MensagemSucesso"] = "Produto cadastrado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        // GET: Produto/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var produto = _produtoRepositorio.ListarPorId(id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }

        // POST: Produto/Alterar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alterar(ProdutoModel produto)
        {
            if (!ModelState.IsValid)
            {
                return View("Editar", produto);
            }

            _produtoRepositorio.Atualizar(produto);

            await RegistrarLogAsync("Alterar", "Produto", $"Produto alterado: {produto.Nome}", produto.Quantidade);

            TempData["MensagemSucesso"] = "Produto atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        // GET: Produto/ApagarConfirmacao/5
        [HttpGet]
        public IActionResult ApagarConfirmacao(int id)
        {
            var produto = _produtoRepositorio.ListarPorId(id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }

        // GET: Produto/Apagar/5
        [HttpGet]
        public async Task<IActionResult> Apagar(int id)
        {
            try
            {
                var produto = _produtoRepositorio.ListarPorId(id);

                if (produto == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado.";
                    return RedirectToAction("Index");
                }

                bool apagado = _produtoRepositorio.Apagar(id);

                if (apagado)
                {
                    await RegistrarLogAsync("Apagar", "Produto", $"Produto apagado: {produto.Nome}", produto.Quantidade);
                    TempData["MensagemSucesso"] = "Produto apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Erro ao tentar apagar o produto.";
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao tentar apagar o produto: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // EXPORTAÇÃO PARA EXCEL
        public IActionResult ExportarParaExcel(DateTime? dataInicio, DateTime? dataFim)
        {
            var produtos = _produtoRepositorio.BuscarTodos();

            if (dataInicio.HasValue)
                produtos = produtos
                    .FindAll(p => p.DataCadastro.HasValue && p.DataCadastro.Value.Date >= dataInicio.Value.Date);

            if (dataFim.HasValue)
                produtos = produtos
                    .FindAll(p => p.DataCadastro.HasValue && p.DataCadastro.Value.Date <= dataFim.Value.Date);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Produtos");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nome";
                worksheet.Cell(1, 3).Value = "Descrição";
                worksheet.Cell(1, 4).Value = "Preço (R$)";
                worksheet.Cell(1, 5).Value = "Quantidade";
                worksheet.Cell(1, 6).Value = "Total (R$)";
                worksheet.Cell(1, 7).Value = "Data de Cadastro";

                int linha = 2;
                foreach (var p in produtos)
                {
                    worksheet.Cell(linha, 1).Value = p.Id;
                    worksheet.Cell(linha, 2).Value = p.Nome;
                    worksheet.Cell(linha, 3).Value = p.Descricao;
                    worksheet.Cell(linha, 4).Value = p.Preco ?? 0m;
                    worksheet.Cell(linha, 5).Value = p.Quantidade ?? 0;

                    decimal total = (p.Preco ?? 0m) * (p.Quantidade ?? 0);
                    worksheet.Cell(linha, 6).Value = total;

                    worksheet.Cell(linha, 7).Value = p.DataCadastro?.ToString("dd/MM/yyyy") ?? "";
                    linha++;
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Column(4).Style.NumberFormat.Format = "R$ #,##0.00";
                worksheet.Column(6).Style.NumberFormat.Format = "R$ #,##0.00";

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Produtos.xlsx"
                    );
                }
            }
        }

        // MÉTODO PRIVADO ASSÍNCRONO PARA REGISTRAR LOG
        private async Task RegistrarLogAsync(string acao, string controller, string descricao, int? quantidade = null)
        {
            var nomeUsuario = _httpContextAccessor.HttpContext?.Session.GetString("NomeUsuario") ?? "Desconhecido";

            await _activityLogger.LogAsync(nomeUsuario, acao, controller, descricao, quantidade);
        }
    }
}
