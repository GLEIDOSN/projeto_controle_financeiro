using FinanceWeb.Data;
using FinanceWeb.Enums;
using FinanceWeb.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FinanceWeb.Services;

public class ReportService
{
    private readonly FinanceWebContext _context;

    public ReportService(FinanceWebContext context)
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
        Document.Create(container =>
        {
            container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(c => ComposeContent(c, contas));

                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf(stream);

        return stream.ToArray();
    }

    private async Task<List<Conta>> ObterContasFiltradasAsync(DateTime? dataInicio, DateTime? dataFim, string tipoConta, string statusConta)
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
            switch(tipoConta)
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

    void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("Relatório de Contas a Receber").Style(titleStyle);
                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{DateTime.Now:d}");
                });
            });
        });
    }

    private void ComposeContent(IContainer container, List<Conta> contas)
    {
        container
            .PaddingVertical(40)
            .Background(Colors.Grey.Lighten3)
            .Column(column =>
            {
                foreach (var conta in contas)
                {
                    column.Item().Text($"Descrição: {conta.Descricao} - Valor: {conta.Valor:C} - Status: {(conta.Pago ? "Pago" : "Não Pago")}");
                }
            });
    }
}
