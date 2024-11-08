using FinanceWeb.Data;
using FinanceWeb.Models;
using FinanceWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceWeb.Services;

public class ContaService : IContaService
{
    private readonly FinanceWebContext _context;

    public ContaService(FinanceWebContext context)
    {
        _context = context;
    }

    public async Task<List<Conta>> GetAllContasAsync()
    {
        return await _context.Contas.ToListAsync();
    }

    public async Task<Conta?> GetContaByIdAsync(int id)
    {
        return await _context.Contas.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddContaAsync(Conta conta)
    {
        _context.Contas.Add(conta);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateContaAsync(Conta conta)
    {
        _context.Attach(conta).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ContaExistsAsync(conta.Id))
                throw new KeyNotFoundException("Conta not found");
            throw;
        }
    }

    public async Task DeleteContaAsync(int id)
    {
        var conta = await _context.Contas.FindAsync(id);
        if (conta != null)
        {
            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ContaExistsAsync(int id)
    {
        return await _context.Contas.AnyAsync(e => e.Id == id);
    }
}