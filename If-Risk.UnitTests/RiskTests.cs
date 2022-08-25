using FluentAssertions;

namespace If_Risk.UnitTests;

public class RiskTests
{
    [Fact]
    public void RiskStruct_createNewRisk_ReturnName()
    {
        Risk test = new Risk("Test", 1);

        test.Name.Should().Be("Test");
        test.YearlyPrice.Should().Be(1);
    }
}
