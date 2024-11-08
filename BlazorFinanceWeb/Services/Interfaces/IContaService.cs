using FinanceWeb.Models;

namespace FinanceWeb.Services.Interfaces;

public interface IContaService
{
    public Task<List<Conta>> GetAllContasAsync();

    public Task<Conta?> GetContaByIdAsync(int id);

    public Task AddContaAsync(Conta conta);

    public Task UpdateContaAsync(Conta conta);

    public Task DeleteContaAsync(int id);

    public Task<bool> ContaExistsAsync(int id);
}
