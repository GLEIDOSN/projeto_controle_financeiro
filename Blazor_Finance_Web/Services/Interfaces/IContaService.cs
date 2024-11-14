using Blazor_Finance_Web.Enums;
using Blazor_Finance_Web.Models;

namespace Blazor_Finance_Web.Services.Interfaces;

public interface IContaService
{
    public Task<List<Conta>> GetAllContasAsync();

    public Task<Conta?> GetContaByIdAsync(int id);

    public Task AddContaAsync(Conta conta);

    public Task UpdateContaAsync(Conta conta);

    public Task DeleteContaAsync(int id);

    public Task<List<Conta>> ObterContasAsync(DateTime? dataInicio, DateTime? dataFim, string statusConta, TipoConta? tipoContaFiltro);

    public Task<bool> ContaExistsAsync(int id);
}
