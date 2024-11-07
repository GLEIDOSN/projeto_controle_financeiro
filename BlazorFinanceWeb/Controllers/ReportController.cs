using FinanceWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceWeb.Controllers;

[ApiController]
[Route("api/report")]
public class ReportController : Controller
{
    private readonly ReportService _reportService;

    public ReportController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("contas-pdf")]
    public async Task<IActionResult> GetReport(
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] string tipoConta,
        [FromQuery] string statusConta)
    {
        // Passe os filtros para o serviço de geração de PDF
        var pdfData = await _reportService.GenerateReportAsync(dataInicio, dataFim, tipoConta, statusConta);

        // Caminho para salvar o PDF
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs", "RelatorioContas.pdf");

        // Verifique se a pasta "pdfs" existe, se não, crie-a
        var pdfDirectory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(pdfDirectory))
        {
            Directory.CreateDirectory(pdfDirectory!);
        }

        // Salva o PDF no caminho especificado
        await System.IO.File.WriteAllBytesAsync(filePath, pdfData);

        // Retorna a URL do arquivo salvo
        return Ok("/pdfs/RelatorioContas.pdf");
    }
}
