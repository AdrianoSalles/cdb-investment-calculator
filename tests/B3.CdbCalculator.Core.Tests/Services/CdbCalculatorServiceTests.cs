using B3.CdbCalculator.Core.Interfaces;
using B3.CdbCalculator.Core.Models;
using B3.CdbCalculator.Core.Services;
using Moq;
using Xunit;

namespace B3.CdbCalculator.Core.Tests.Services;

public sealed class CdbCalculatorServiceTests
{
    private readonly Mock<ITaxRateService> _taxRateServiceMock = new();
    private readonly CdbCalculatorService _sut;

    public CdbCalculatorServiceTests()
    {
        _sut = new CdbCalculatorService(_taxRateServiceMock.Object);
    }

    [Fact]
    public void Calculate_WithTwoMonths_CompoundsMonthlyRateTwice()
    {
        // M1: 1000 × 1,00972 = 1009,72
        // M2: 1009,72 × 1,00972 = 1019,53 (arredondado)
        _taxRateServiceMock.Setup(s => s.GetTaxRate(2)).Returns(0.225m);

        var result = _sut.Calculate(new CdbCalculationInput(1000m, 2));

        Assert.Equal(1019.53m, result.GrossAmount);
    }

    [Fact]
    public void Calculate_GrossEarnings_IsGrossAmountMinusInitialAmount()
    {
        _taxRateServiceMock.Setup(s => s.GetTaxRate(It.IsAny<int>())).Returns(0.20m);

        var result = _sut.Calculate(new CdbCalculationInput(1000m, 12));

        Assert.Equal(result.GrossAmount - result.InitialAmount, result.GrossEarnings);
    }

    [Fact]
    public void Calculate_TaxAmount_IsGrossEarningsMultipliedByTaxRate()
    {
        const decimal initialAmount = 1_000m;
        const int months = 2;
        const decimal taxRate = 0.225m;

        _taxRateServiceMock.Setup(service => service.GetTaxRate(months)).Returns(taxRate);

        var input = new CdbCalculationInput(initialAmount, months);

        var result = _sut.Calculate(input);

        var expectedTaxAmount = decimal.Round(
            result.GrossEarnings * taxRate,
            2,
            MidpointRounding.AwayFromZero);

        Assert.Equal(expectedTaxAmount, result.TaxAmount);
    }

    [Fact]
    public void Calculate_WithOneThousandForTwelveMonths_ReturnsExpectedValues()
    {
        var sut = new CdbCalculatorService(new TaxRateService());

        var result = sut.Calculate(
            new CdbCalculationInput(
                InitialAmount: 1_000m,
                Months: 12));

        Assert.Equal(1_000m, result.InitialAmount);
        Assert.Equal(12, result.Months);
        Assert.Equal(1_123.08m, result.GrossAmount);
        Assert.Equal(123.08m, result.GrossEarnings);
        Assert.Equal(0.20m, result.TaxRate);
        Assert.Equal(24.62m, result.TaxAmount);
        Assert.Equal(1_098.46m, result.NetAmount);
    }

    [Fact]
    public void Calculate_NetAmount_IsGrossAmountMinusTaxAmount()
    {
        const decimal initialAmount = 1_000m;
        const int months = 2;
        const decimal taxRate = 0.225m;

        _taxRateServiceMock.Setup(service => service.GetTaxRate(months)).Returns(taxRate);

        var input = new CdbCalculationInput(initialAmount, months);

        var result = _sut.Calculate(input);

        var expectedNetAmount = result.GrossAmount - result.TaxAmount;

        Assert.Equal(expectedNetAmount, result.NetAmount);
    }

    [Fact]
    public void Calculate_DelegatesToTaxRateServiceWithCorrectMonths()
    {
        _taxRateServiceMock.Setup(s => s.GetTaxRate(18)).Returns(0.175m);

        _sut.Calculate(new CdbCalculationInput(1000m, 18));

        _taxRateServiceMock.Verify(s => s.GetTaxRate(18), Times.Once);
    }

    [Fact]
    public void Calculate_UsesTaxRateReturnedByService()
    {
        const decimal customRate = 0.10m;
        _taxRateServiceMock.Setup(s => s.GetTaxRate(It.IsAny<int>())).Returns(customRate);

        var result = _sut.Calculate(new CdbCalculationInput(1000m, 6));

        Assert.Equal(customRate, result.TaxRate);
    }

    [Fact]
    public void Calculate_WhenInputIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => _sut.Calculate(null!));
    }

    [Fact]
    public void Calculate_WhenTaxIsHalfCent_RoundsAwayFromZero()
    {
        _taxRateServiceMock
            .Setup(service => service.GetTaxRate(2))
            .Returns(0.25m);

        var result = _sut.Calculate(
            new CdbCalculationInput(
                InitialAmount: 1m,
                Months: 2));

        Assert.Equal(0.01m, result.TaxAmount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_WhenInitialAmountIsNotPositive_Throws(
        decimal initialAmount)
    {
        var input = new CdbCalculationInput(initialAmount, 12);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => _sut.Calculate(input));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_WhenMonthsIsLessThanTwo_Throws(int months)
    {
        var input = new CdbCalculationInput(1000m, months);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => _sut.Calculate(input));
    }
}
