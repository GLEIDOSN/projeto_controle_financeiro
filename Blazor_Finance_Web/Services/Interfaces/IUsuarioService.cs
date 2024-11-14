using Blazor_Finance_Web.Models;

namespace Blazor_Finance_Web.Services.Interfaces;

public interface IUsuarioService
{
    public Task<Usuario> AddUsuarioAsync(Usuario usuario);

    public Task<Usuario?> GetUsuarioByIdAsync(int id);

    public Task<List<Usuario>> GetAllUsuariosAsync();

    public Task<bool> ExisteUsuarioAsync(int id);

    public Task UpdateUsuarioAsync(Usuario usuario);

    public Task AtualizarUsuarioAsync(Usuario usuario, string? novaSenha);

    public Task DeleteUsuarioAsync(int id);

    public Task<Usuario?> AutenticarAsync(string email, string senha);
}
