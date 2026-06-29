using System.ComponentModel.DataAnnotations;

namespace B3.CdbCalculator.Api.DTOs;

/// <summary>
/// Dados enviados pelo cliente para cálculo do CDB.
/// </summary>
public sealed class CdbCalculationRequest
{
    /// <summary>Valor inicial do investimento. Deve ser maior que zero.</summary>
    [Range(0.01, (double)decimal.MaxValue,
        ErrorMessage = "O valor inicial deve ser maior que zero.")]
    public decimal InitialAmount { get; init; }

    /// <summary>Prazo em meses. Deve ser maior que 1.</summary>
    [Range(2, int.MaxValue,
        ErrorMessage = "O prazo deve ser maior que 1 mês.")]
    public int Months { get; init; }
}
