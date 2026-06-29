using B3.CdbCalculator.Core.Services;
using Xunit;

namespace B3.CdbCalculator.Core.Tests.Services;

public sealed class TaxRateServiceTests
{
    private readonly TaxRateService _sut = new();

    public static TheoryData<int, decimal> ValidTaxRateCases =>
        new()
        {
        { 2, 0.225m },
        { 6, 0.225m },
        { 7, 0.20m },
        { 12, 0.20m },
        { 13, 0.175m },
        { 24, 0.175m },
        { 25, 0.15m },
        { 120, 0.15m },
        };

    [Theory]
    [MemberData(nameof(ValidTaxRateCases))]
    public void GetTaxRate_WhenMonthsIsValid_ReturnsExpectedRate(
        int months,
        decimal expectedRate)
    {
        var rate = _sut.GetTaxRate(months);

        Assert.Equal(expectedRate, rate);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void GetTaxRate_WhenMonthsIsLessThanTwo_Throws(
        int months)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => _sut.GetTaxRate(months));
    }
}
