using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemaControleEstoque.Models;

namespace SistemaControleEstoque.Helper
{
    public static class PdfReportHelper
    {
        static PdfReportHelper()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static byte[] GerarPdfProdutos(List<ProdutoModel> produtos, DateTime? dataInicio, DateTime? dataFim)
        {
            decimal totalGeral = produtos.Sum(p => (p.Preco ?? 0m) * (decimal)(p.Quantidade ?? 0));
            int totalItens = produtos.Sum(p => p.Quantidade ?? 0);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().Element(c => MontarHeader(c, "Relatório de Produtos em Estoque", dataInicio, dataFim));

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);  // ID
                            columns.RelativeColumn(3);   // Nome
                            columns.RelativeColumn(3);   // Descrição
                            columns.RelativeColumn(2);   // Categoria
                            columns.RelativeColumn(1.5f);// Preço
                            columns.RelativeColumn(1.2f);// Qtd
                            columns.RelativeColumn(1.8f);// Total
                            columns.RelativeColumn(2);   // Data Cadastro
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(EstiloCabecalho).Text("ID");
                            header.Cell().Element(EstiloCabecalho).Text("Nome");
                            header.Cell().Element(EstiloCabecalho).Text("Descrição");
                            header.Cell().Element(EstiloCabecalho).Text("Categoria");
                            header.Cell().Element(EstiloCabecalho).Text("Preço");
                            header.Cell().Element(EstiloCabecalho).Text("Qtd");
                            header.Cell().Element(EstiloCabecalho).Text("Total");
                            header.Cell().Element(EstiloCabecalho).Text("Data Cad.");
                        });

                        for (int i = 0; i < produtos.Count; i++)
                        {
                            var p = produtos[i];
                            var bg = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;
                            decimal itemTotal = (p.Preco ?? 0m) * (p.Quantidade ?? 0);

                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(p.Id.ToString());
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(p.Nome ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(p.Descricao ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(p.Categoria?.Nome ?? "-");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text((p.Preco ?? 0m).ToString("C"));
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text((p.Quantidade ?? 0).ToString());
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(itemTotal.ToString("C"));
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(p.DataCadastro?.ToString("dd/MM/yyyy") ?? "");
                        }

                        // Linha de total
                        table.Cell().ColumnSpan(5).Element(c => EstiloTotal(c)).AlignRight().Text("Totais:");
                        table.Cell().Element(c => EstiloTotal(c)).Text(totalItens.ToString());
                        table.Cell().Element(c => EstiloTotal(c)).Text(totalGeral.ToString("C"));
                        table.Cell().Element(c => EstiloTotal(c)).Text("");
                    });

                    page.Footer().Element(MontarFooter);
                });
            });

            return document.GeneratePdf();
        }

        public static byte[] GerarPdfUsuarios(List<UsuarioModel> usuarios, DateTime? dataInicio, DateTime? dataFim)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().Element(c => MontarHeader(c, "Relatório de Usuários", dataInicio, dataFim));

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2.5f);
                            columns.RelativeColumn(3.5f);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(EstiloCabecalho).Text("ID");
                            header.Cell().Element(EstiloCabecalho).Text("Nome");
                            header.Cell().Element(EstiloCabecalho).Text("Login");
                            header.Cell().Element(EstiloCabecalho).Text("E-mail");
                            header.Cell().Element(EstiloCabecalho).Text("Perfil");
                            header.Cell().Element(EstiloCabecalho).Text("Data Cad.");
                        });

                        for (int i = 0; i < usuarios.Count; i++)
                        {
                            var u = usuarios[i];
                            var bg = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;

                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(u.Id.ToString());
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(u.Nome ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(u.Login ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(u.Email ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(u.Perfil?.ToString() ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(u.DataCadastro?.ToString("dd/MM/yyyy") ?? "");
                        }
                    });

                    page.Footer().Element(MontarFooter);
                });
            });

            return document.GeneratePdf();
        }

        public static byte[] GerarPdfClientes(List<ClienteModel> clientes, DateTime? dataInicio, DateTime? dataFim)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().Element(c => MontarHeader(c, "Relatório de Clientes", dataInicio, dataFim));

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2.5f);
                            columns.RelativeColumn(2.5f);
                            columns.RelativeColumn(2.5f);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(EstiloCabecalho).Text("ID");
                            header.Cell().Element(EstiloCabecalho).Text("Nome");
                            header.Cell().Element(EstiloCabecalho).Text("E-mail");
                            header.Cell().Element(EstiloCabecalho).Text("Telefone");
                            header.Cell().Element(EstiloCabecalho).Text("Nascimento");
                            header.Cell().Element(EstiloCabecalho).Text("Data Cad.");
                        });

                        for (int i = 0; i < clientes.Count; i++)
                        {
                            var cl = clientes[i];
                            var bg = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;

                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(cl.Id.ToString());
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(cl.Nome ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(cl.Email ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(cl.Telefone ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(cl.DataNascimento?.ToString("dd/MM/yyyy") ?? "-");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(cl.DataCadastro?.ToString("dd/MM/yyyy") ?? "-");
                        }
                    });

                    page.Footer().Element(MontarFooter);
                });
            });

            return document.GeneratePdf();
        }

        public static byte[] GerarPdfLogs(List<ActivityLog> logs, DateTime? dataInicio, DateTime? dataFim)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().Element(c => MontarHeader(c, "Registro de Atividades do Sistema", dataInicio, dataFim));

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2.5f); // Usuário
                            columns.RelativeColumn(1.8f); // Ação
                            columns.RelativeColumn(2);    // Controller
                            columns.RelativeColumn(4.5f); // Descrição
                            columns.RelativeColumn(1.2f); // Quantidade
                            columns.RelativeColumn(2.2f); // Data/Hora
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(EstiloCabecalho).Text("Usuário");
                            header.Cell().Element(EstiloCabecalho).Text("Ação");
                            header.Cell().Element(EstiloCabecalho).Text("Controller");
                            header.Cell().Element(EstiloCabecalho).Text("Descrição");
                            header.Cell().Element(EstiloCabecalho).Text("Qtd");
                            header.Cell().Element(EstiloCabecalho).Text("Data/Hora");
                        });

                        for (int i = 0; i < logs.Count; i++)
                        {
                            var l = logs[i];
                            var bg = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;

                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(l.UserName ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(l.Action ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(l.Controller ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(l.Description ?? "");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(l.Quantidade.HasValue ? l.Quantidade.Value.ToString() : "-");
                            table.Cell().Element(c => EstiloCelula(c, bg)).Text(l.Timestamp.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss"));
                        }
                    });

                    page.Footer().Element(MontarFooter);
                });
            });

            return document.GeneratePdf();
        }

        private static void MontarHeader(IContainer container, string titulo, DateTime? dataInicio, DateTime? dataFim)
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(titleCol =>
                    {
                        titleCol.Item().Text("Gestor360 - Sistema de Controle de Estoque")
                            .FontSize(14).Bold().FontColor("#003366");
                        titleCol.Item().Text(titulo)
                            .FontSize(12).SemiBold().FontColor("#334155");
                    });

                    row.RelativeItem().AlignRight().Column(infoCol =>
                    {
                        infoCol.Item().Text($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(9).FontColor("#64748b");

                        if (dataInicio.HasValue || dataFim.HasValue)
                        {
                            string periodo = $"Período: {(dataInicio.HasValue ? dataInicio.Value.ToString("dd/MM/yyyy") : "Início")} até {(dataFim.HasValue ? dataFim.Value.ToString("dd/MM/yyyy") : "Hoje")}";
                            infoCol.Item().Text(periodo).FontSize(9).FontColor("#64748b");
                        }
                    });
                });

                col.Item().PaddingTop(5).LineHorizontal(1).LineColor("#003366");
            });
        }

        private static void MontarFooter(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                col.Item().PaddingTop(4).Row(row =>
                {
                    row.RelativeItem().Text("Gestor360 - CL Soluções Tecnológicas")
                        .FontSize(8).FontColor(Colors.Grey.Medium);

                    row.RelativeItem().AlignRight().Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            });
        }

        private static IContainer EstiloCabecalho(IContainer container)
        {
            return container
                .Background("#003366")
                .Padding(5)
                .DefaultTextStyle(x => x.SemiBold().FontColor(Colors.White).FontSize(9));
        }

        private static IContainer EstiloCelula(IContainer container, string backgroundColor)
        {
            return container
                .Background(backgroundColor)
                .Padding(5)
                .BorderBottom(0.5f)
                .BorderColor(Colors.Grey.Lighten3)
                .DefaultTextStyle(x => x.FontSize(8.5f));
        }

        private static IContainer EstiloTotal(IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten3)
                .Padding(5)
                .DefaultTextStyle(x => x.Bold().FontSize(9).FontColor("#003366"));
        }
    }
}
