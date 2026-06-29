using B3.CdbCalculator.Core.Interfaces;

namespace B3.CdbCalculator.Core.Services;

/// <summary>
/// Determina a alíquota de Imposto de Renda aplicável ao CDB
/// com base no prazo de resgate, conforme tabela regressiva da Receita Federal.
/// </summary>
public sealed class TaxRateService : ITaxRateService
{
    /// <inheritdoc />
    public decimal GetTaxRate(int months)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(months, 2);

        return months switch
        {
            <= 6 => 0.225m,
            <= 12 => 0.20m,
            <= 24 => 0.175m,
            _ => 0.15m,
        };
    }
}
