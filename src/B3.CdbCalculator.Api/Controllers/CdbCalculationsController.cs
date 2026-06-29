using B3.CdbCalculator.Api.DTOs;
using B3.CdbCalculator.Core.Interfaces;
using B3.CdbCalculator.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace B3.CdbCalculator.Api.Controllers;

/// <summary>
/// Endpoint para cálculo de investimentos em CDB.
/// </summary>
[ApiController]
[Route("api/cdb/calculations")]
public sealed class CdbCalculationsController(ICdbCalculatorService calculatorService) : ControllerBase
{
    private readonly ICdbCalculatorService _calculatorService = calculatorService;

    /// <summary>
    /// Calcula o rendimento bruto e líquido de um investimento em CDB.
    /// </summary>
    /// <param name="request">Valor inicial e prazo em meses.</param>
    /// <returns>Resultado com valores bruto, líquido e imposto aplicado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CdbCalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<CdbCalculationResponse> Calculate([FromBody] CdbCalculationRequest request)
    {
        var output = _calculatorService.Calculate(
            new CdbCalculationInput(request.InitialAmount, request.Months));

        return Ok(CdbCalculationResponse.FromOutput(output));
    }
}
