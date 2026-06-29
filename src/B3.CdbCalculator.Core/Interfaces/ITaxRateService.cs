namespace B3.CdbCalculator.Core.Interfaces;

/// <summary>
/// Define o contrato para determinação da alíquota de Imposto de Renda
/// sobre rendimentos de CDB conforme prazo de resgate.
/// </summary>
public interface ITaxRateService
{
    /// <summary>
    /// Retorna a alíquota de IR aplicável ao prazo informado.
    /// </summary>
    /// <param name="months">Prazo em meses do investimento.</param>
    /// <returns>Alíquota como fração decimal (ex: 0,20 para 20%).</returns>
    public decimal GetTaxRate(int months);
}
