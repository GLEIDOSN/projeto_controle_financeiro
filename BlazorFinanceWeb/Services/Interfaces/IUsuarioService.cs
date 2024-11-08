using FinanceWeb.Models;

namespace FinanceWeb.Services.Interfaces;

public interface IUsuarioService
{
    public Task<Usuario> AdicionarUsuarioAsync(Usuario usuario);

    public Task<IEnumerable<Usuario>> ObterUsuariosAsync();

    public Task<Usuario?> ObterUsuarioPorIdAsync(int id);

    public Task<List<Usuario>> ObterTodosAsync();

    public Task<bool> ExisteUsuarioAsync(int id);

    public Task AtualizarUsuarioAsync(Usuario usuario);

    public Task AtualizarUsuarioAsync(Usuario usuario, string? novaSenha);

    public Task RemoverUsuarioAsync(int id);

    public Task<Usuario?> AutenticarAsync(string email, string senha);
}
