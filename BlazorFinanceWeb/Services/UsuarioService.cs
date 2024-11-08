﻿using FinanceWeb.Data;
using FinanceWeb.Models;
using FinanceWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceWeb.Services;

public class UsuarioService : IUsuarioService
{
    private readonly FinanceWebContext _context;

    public UsuarioService(FinanceWebContext context)
    {
        _context = context;
    }

    public async Task<Usuario> AdicionarUsuarioAsync(Usuario usuario)
    {
        usuario.SenhaHash = HashPassword(usuario.Senha);
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task AtualizarUsuarioAsync(Usuario usuario)
    {
        usuario.SenhaHash = HashPassword(usuario.Senha);
        _context.Usuarios.Update(usuario);

        await _context.SaveChangesAsync();
    }

    public async Task<Usuario?> AutenticarAsync(string email, string senha)
    {
        var usuario = await _context.Set<Usuario>()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (usuario != null && VerifyPassword(senha, usuario.SenhaHash))
        {
            return usuario;
        }

        return null;
    }

    public async Task<Usuario?> ObterUsuarioPorIdAsync(int id)
    {
        return await _context.Set<Usuario>().FindAsync(id);
    }

    public async Task<IEnumerable<Usuario>> ObterUsuariosAsync()
    {
        return await _context.Set<Usuario>().ToListAsync();
    }

    public async Task<List<Usuario>> ObterTodosAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task RemoverUsuarioAsync(int id)
    {
        var usuario = await ObterUsuarioPorIdAsync(id);
            
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);

            await _context.SaveChangesAsync();
        }
    }

    public async Task AtualizarUsuarioAsync(Usuario usuario, string? novaSenha)
    {
        var usuarioDb = await _context.Usuarios.FindAsync(usuario.Id);
        if (usuarioDb == null) throw new ArgumentException("Usuário não encontrado");

        usuarioDb.Nome = usuario.Nome;
        usuarioDb.Email = usuario.Email;

        if (!string.IsNullOrEmpty(novaSenha))
        {
            usuarioDb.SenhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteUsuarioAsync(int id)
    {
        return await _context.Usuarios.AnyAsync(e => e.Id == id);
    }

    // Método auxiliar para hash de senha usando BCrypt
    private string HashPassword(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    // Método auxiliar para verificar a senha usando BCrypt
    private bool VerifyPassword(string senha, string senhaHash)
    {
        return BCrypt.Net.BCrypt.Verify(senha, senhaHash);
    }
}