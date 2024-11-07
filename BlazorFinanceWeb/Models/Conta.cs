using FinanceWeb.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FinanceWeb.Models;

public class Conta
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo Descrição é obrigatório.")]
    public string Descricao { get; set; } = string.Empty;

    [DataType(DataType.Currency)]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
    [Display(Name = "Data de Vencimento")]
    public DateTime DataVencimento { get; set; }

    public bool Pago { get; set; }

    [Required(ErrorMessage = "O Tipo de conta é obrigatório.")]
    public TipoConta Tipo { get; set; }

    [NotMapped]
    public string ValorFormatado => Valor.ToString("C2", CultureInfo.CurrentCulture);

    [NotMapped]
    public string TipoFormatado => Tipo == TipoConta.Pagar ? "Pagar" : "Receber";

    [NotMapped]
    public string DataVencimentoFormatado => DataVencimento.ToString("dd/MM/yyyy");
}