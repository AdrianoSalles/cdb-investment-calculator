namespace B3.CdbCalculator.Core.Models;

/// <summary>
/// Dados de entrada para o cálculo do CDB.
/// </summary>
/// <param name="InitialAmount">Valor inicial do investimento. Deve ser maior que zero.</param>
/// <param name="Months">Prazo em meses. Deve ser maior que 1.</param>
public sealed record CdbCalculationInput(decimal InitialAmount, int Months);
