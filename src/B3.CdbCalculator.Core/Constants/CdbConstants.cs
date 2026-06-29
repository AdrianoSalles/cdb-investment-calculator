using System.Diagnostics.CodeAnalysis;

namespace B3.CdbCalculator.Core.Constants;

/// <summary>
/// Parâmetros fixos utilizados no cálculo do CDB conforme especificação do exercício.
/// </summary>
[ExcludeFromCodeCoverage(
    Justification = "Contains only compile-time constants and no executable business logic.")]
internal static class CdbConstants
{
    /// <summary>Taxa CDI mensal: 0,9%.</summary>
    internal const decimal Cdi = 0.009m;

    /// <summary>Taxa do banco sobre o CDI: 108%.</summary>
    internal const decimal Tb = 1.08m;

    /// <summary>Rendimento mensal composto: CDI × TB = 0,00972.</summary>
    internal const decimal MonthlyRate = Cdi * Tb;
}
