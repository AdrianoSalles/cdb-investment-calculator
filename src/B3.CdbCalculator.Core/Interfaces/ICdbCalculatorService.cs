using B3.CdbCalculator.Core.Models;

namespace B3.CdbCalculator.Core.Interfaces;

/// <summary>
/// Define o contrato para cálculo de rendimento de CDB.
/// </summary>
public interface ICdbCalculatorService
{
    /// <summary>
    /// Calcula o rendimento bruto e líquido de um investimento em CDB.
    /// </summary>
    /// <param name="input">Dados de entrada com valor inicial e prazo.</param>
    /// <returns>Resultado com valores bruto, líquido e imposto aplicado.</returns>
    public CdbCalculationOutput Calculate(CdbCalculationInput input);
}
