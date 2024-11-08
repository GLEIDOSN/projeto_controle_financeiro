using FinanceWeb.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceWeb.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O Tipo de usuário é obrigatório.")]
    public TipoUsuario TipoUsuario { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um email válido.")]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Senha é obrigatório.")]
    public string Senha { get; set; } = string.Empty;

    // A senha criptografada será salva no banco
    public string SenhaHash { get; set; } = string.Empty;

    [NotMapped]
    public string TipoUsuarioFormatado => TipoUsuario == TipoUsuario.Admin? "Admin" : "Operador";
}
