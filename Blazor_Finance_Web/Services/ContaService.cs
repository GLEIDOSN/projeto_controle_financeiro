using Blazor_Finance_Web.Data;
using Blazor_Finance_Web.Enums;
using Blazor_Finance_Web.Models;
using Blazor_Finance_Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Finance_Web.Services;

public class ContaService : IContaService
{
    private readonly ApplicationDbContext _context;

    public ContaService(ApplicationDbContext context)
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

    public async Task<List<Conta>> ObterContasAsync(DateTime? dataInicio, DateTime? dataFim, string statusConta, TipoConta? tipoContaFiltro)
    {
        var query = _context.Set<Conta>().AsQueryable();

        if (dataInicio.HasValue)
        {
            query = query.Where(c => c.DataVencimento >= dataInicio.Value);
        }

        if (dataFim.HasValue)
        {
            query = query.Where(c => c.DataVencimento <= dataFim.Value);
        }

        if (!string.IsNullOrEmpty(statusConta) && statusConta != "Todos")
        {
            query = statusConta switch
            {
                "Pendente" => query.Where(c => !c.Pago),
                "Pago" => query.Where(c => c.Pago),
                _ => query
            };
        }

        if (tipoContaFiltro.HasValue)
        {
            query = query.Where(c => c.Tipo == tipoContaFiltro);
        }

        return await query.ToListAsync();
    }

    public async Task<bool> ContaExistsAsync(int id)
    {
        return await _context.Contas.AnyAsync(e => e.Id == id);
    }
}
