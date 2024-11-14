using Blazor_Finance_Web.Data;
using Blazor_Finance_Web.Enums;
using Blazor_Finance_Web.Models;
using Blazor_Finance_Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Blazor_Finance_Web.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> GenerateReportAsync(DateTime? dataInicio, DateTime? dataFim, string tipoConta, string statusConta)
    {
        using var stream = new MemoryStream();

        // Aqui você pode usar os filtros para buscar e filtrar os dados necessários no banco de dados.
        // Por exemplo:
        var contas = await ObterContasFiltradasAsync(dataInicio, dataFim, tipoConta, statusConta);

        // Criação do documento PDF com os dados filtrados
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                page.Header()
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Column(column =>
                            {
                                column.Item()
                                    .Text("Sistema Financeiro Web")
                                    .FontFamily("Arial")
                                    .FontSize(20)
                                    .Bold();
                                column.Item()
                                    .Text("Rua Dodoria da Silva, 964 - Fortaleza - CE")
                                    .FontFamily("Arial")
                                    .FontSize(15);
                            });

                        row.RelativeItem()
                            .ShowOnce()
                            .Text("Contas a Pagar/Receber")
                            .AlignRight()
                            .FontFamily("Arial")
                            .ExtraBlack()
                            .FontSize(20);
                    });

                page.Content()
                    .PaddingTop(5)
                    .Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem()
                                .Column(column2 =>
                                {
                                    column2.Item()
                                        .Text("Filtros:")
                                        .FontSize(15)
                                        .Bold();
                                    column2.Item()
                                        .Text($"{dataInicio:dd/MM/yyyy}-{dataFim:dd/MM/yyyy}-{tipoConta}-{statusConta}")
                                        .FontFamily("Arial")
                                        .FontSize(12);
                                });

                            row.RelativeItem()
                                .Column(column2 =>
                                {
                                    column2.Item()
                                        .Text("Data:")
                                        .FontSize(15)
                                        .Bold();
                                    column2.Item()
                                        .Text($"{DateTime.Now:dd/mm/yyyy}")
                                        .FontFamily("Arial")
                                        .FontSize(12);
                                });

                            column.Item().PaddingTop(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(40); // Id
                                    columns.RelativeColumn();   // Descrição
                                    columns.ConstantColumn(80); // Vencimento
                                    columns.ConstantColumn(60); // Tipo
                                    columns.ConstantColumn(70); // Valor
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Padding(4).Text("Id").Bold();
                                    header.Cell().Padding(4).Text("Descrição").Bold();
                                    header.Cell().Padding(4).Text("Vencimento").Bold();
                                    header.Cell().Padding(4).Text("Tipo").Bold();
                                    header.Cell().Padding(4).Text("Valor").AlignRight().Bold();

                                    header.Cell()
                                        .ColumnSpan(5)
                                        .PaddingVertical(5)
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Black);
                                });

                                for (var i = 0; i < contas.Count; i++)
                                {
                                    var background = i % 2 == 0 ?
                                        Color.FromHex("#ffffff") :
                                        Color.FromHex("#f0f0f0");

                                    var conta = contas[i];
                                    table.Cell().ShowEntire().Background(background).Padding(4).Text(conta.Id.ToString());
                                    table.Cell().ShowEntire().Background(background).Padding(4).Text(conta.Descricao);
                                    table.Cell().ShowEntire().Background(background).Padding(4).Text(conta.DataVencimentoFormatado);
                                    table.Cell().ShowEntire().Background(background).Padding(4).Text(conta.TipoFormatado);
                                    table.Cell().ShowEntire().Background(background).Padding(4).AlignRight().Text(conta.Valor.ToString("N2"));
                                }

                                table.Cell()
                                    .ColumnSpan(5)
                                    .PaddingVertical(5)
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Black);
                                table.Cell().ColumnSpan(4).Text("Total").Bold().AlignRight();
                                table.Cell().AlignRight().Text(contas.Sum(x => x.Valor).ToString("N2")).Bold().AlignRight();
                            });
                        });
                    });

                page.Footer()
                    .Column(column =>
                    {
                        column.Item()
                            .PaddingVertical(10)
                            .Text(text =>
                            {
                                text.Span("Page ");
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                                text.AlignCenter();
                            });
                    });
            });
        });

        //document.ShowInCompanion();
        document.GeneratePdf(stream);

        return stream.ToArray();
    }

    public async Task<List<Conta>> ObterContasFiltradasAsync(DateTime? dataInicio, DateTime? dataFim, string tipoConta, string statusConta)
    {
        var query = _context.Contas.AsQueryable();

        if (dataInicio.HasValue)
        {
            query = query.Where(c => c.DataVencimento >= dataInicio.Value);
        }
        if (dataFim.HasValue)
        {
            query = query.Where(c => c.DataVencimento <= dataFim.Value);
        }
        if (!string.IsNullOrEmpty(tipoConta) && tipoConta != "Todos")
        {
            switch (tipoConta)
            {
                case "Pagar": query = query.Where(c => c.Tipo == TipoConta.Pagar); break;
                case "Receber": query = query.Where(c => c.Tipo == TipoConta.Receber); break;
            }
        }
        if (!string.IsNullOrEmpty(statusConta) && statusConta != "Todos")
        {
            switch (statusConta)
            {
                case "Pago": query = query.Where(c => c.Pago == true); break;
            }
        }

        return await query.ToListAsync();
    }
}
