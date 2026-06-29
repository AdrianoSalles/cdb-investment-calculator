using B3.CdbCalculator.Core.Models;

namespace B3.CdbCalculator.Api.DTOs;

/// <summary>
/// Resultado do cálculo retornado ao cliente.
/// </summary>
/// <param name="InitialAmount">Valor inicial investido.</param>
/// <param name="Months">Prazo em meses.</param>
/// <param name="GrossAmount">Valor bruto ao final do período.</param>
/// <param name="GrossEarnings">Rendimento bruto.</param>
/// <param name="TaxRate">Alíquota de IR aplicada (ex: 0,20 para 20%).</param>
/// <param name="TaxAmount">Valor do imposto sobre o rendimento.</param>
/// <param name="NetAmount">Valor líquido após dedução do imposto.</param>
public sealed record CdbCalculationResponse(
    decimal InitialAmount,
    int Months,
    decimal GrossAmount,
    decimal GrossEarnings,
    decimal TaxRate,
    decimal TaxAmount,
    decimal NetAmount)
{
    public static CdbCalculationResponse FromOutput(
        CdbCalculationOutput output)
    {
        return new(
            output.InitialAmount,
            output.Months,
            output.GrossAmount,
            output.GrossEarnings,
            output.TaxRate,
            output.TaxAmount,
            output.NetAmount);
    }
}
