using Blazor_Finance_Web.Models;

namespace Blazor_Finance_Web.Services.Interfaces;

public interface IReportService
{
    public Task<byte[]> GenerateReportAsync(DateTime? dataInicio, DateTime? dataFim, string tipoConta, string statusConta);

    public Task<List<Conta>> ObterContasFiltradasAsync(DateTime? dataInicio, DateTime? dataFim, string tipoConta, string statusConta);
}
