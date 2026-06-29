namespace B3.CdbCalculator.Core.Models;

/// <summary>
/// Resultado do cálculo do CDB com valores bruto, líquido e imposto.
/// </summary>
/// <param name="InitialAmount">Valor inicial investido.</param>
/// <param name="Months">Prazo em meses.</param>
/// <param name="GrossAmount">Valor bruto ao final do período.</param>
/// <param name="GrossEarnings">Rendimento bruto (GrossAmount - InitialAmount).</param>
/// <param name="TaxRate">Alíquota de imposto aplicada (ex: 0,20 para 20%).</param>
/// <param name="TaxAmount">Valor do imposto sobre o rendimento.</param>
/// <param name="NetAmount">Valor líquido após o imposto (GrossAmount - TaxAmount).</param>
public sealed record CdbCalculationOutput(
    decimal InitialAmount,
    int Months,
    decimal GrossAmount,
    decimal GrossEarnings,
    decimal TaxRate,
    decimal TaxAmount,
    decimal NetAmount);
