using B3.CdbCalculator.Core.Constants;
using B3.CdbCalculator.Core.Interfaces;
using B3.CdbCalculator.Core.Models;

namespace B3.CdbCalculator.Core.Services;

/// <summary>
/// Calcula o rendimento de CDB aplicando a fórmula de capitalização composta
/// mês a mês: VF = VI × [1 + (CDI × TB)], repetida para cada mês do prazo.
/// O imposto incide apenas sobre os rendimentos, nunca sobre o principal.
/// </summary>
public sealed class CdbCalculatorService(ITaxRateService taxRateService) : ICdbCalculatorService
{
    private readonly ITaxRateService _taxRateService = taxRateService;

    /// <inheritdoc />
    public CdbCalculationOutput Calculate(
        CdbCalculationInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(
            input.InitialAmount);
        ArgumentOutOfRangeException.ThrowIfLessThan(
            input.Months,
            2);

        var rawGrossAmount = CalculateGrossAmount(
            input.InitialAmount,
            input.Months);

        var grossAmount = RoundMoney(rawGrossAmount);

        var grossEarnings = RoundMoney(
            grossAmount - input.InitialAmount);

        var taxRate = _taxRateService.GetTaxRate(
            input.Months);

        var taxAmount = RoundMoney(
            grossEarnings * taxRate);

        var netAmount = RoundMoney(grossAmount - taxAmount);

        return new CdbCalculationOutput(
            input.InitialAmount,
            input.Months,
            grossAmount,
            grossEarnings,
            taxRate,
            taxAmount,
            netAmount);
    }

    private static decimal CalculateGrossAmount(
        decimal initialAmount,
        int months)
    {
        var amount = initialAmount;
        var monthlyFactor = 1m + CdbConstants.MonthlyRate;

        for (var month = 0; month < months; month++)
        {
            amount *= monthlyFactor;
        }

        return amount;
    }

    private static decimal RoundMoney(decimal value) =>
        decimal.Round(value, 2, MidpointRounding.AwayFromZero);
}
