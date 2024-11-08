using FinanceWeb.Models;

namespace FinanceWeb.Services.Interfaces;

public interface IReportService
{
    public Task<byte[]> GenerateReportAsync(DateTime? dataInicio, DateTime? dataFim, string tipoConta, string statusConta);

    public Task<List<Conta>> ObterContasFiltradasAsync(DateTime? dataInicio, DateTime? dataFim, string tipoConta, string statusConta);
}
