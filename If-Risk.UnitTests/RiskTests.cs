using FluentAssertions;

namespace If_Risk.UnitTests;

public class RiskTests
{
    [Fact]
    public void RiskStruct_createNewRisk_ReturnName()
    {
        // Arrange
        Risk test = new Risk("Test", 1);

        // Act
        var testName = test.Name == "Test";

        // Assert
        test.Name.Should().Be("Test");
        test.YearlyPrice.Should().Be(1);
        testName.Should().BeTrue();
    }
}
