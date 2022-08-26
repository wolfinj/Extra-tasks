using FluentAssertions;

namespace If_Risk.UnitTests;

public class PolicyTests
{
    private readonly List<Risk> _risks;
    private readonly Policy _homePolicy;
    private readonly DateTime _startDate = new(2024, 02, 01);
    private readonly DateTime _endDate = new(2024, 08, 01);

    public PolicyTests()
    {
        _risks = new List<Risk>
        {
            new("Theft", 200),
            new("Flood", 300),
            new("Aliens", 1200),
            new("Ants", 100)
        };
        _homePolicy = new Policy("18 tree st.", _startDate, _endDate, _risks);
    }

    [Fact]
    public void Policy_CreateNewPolicy_CheckNameAndRisks()
    {
        _homePolicy.NameOfInsuredObject.Should().Be("18 tree st.");
        _homePolicy.InsuredRisks.Should().BeEquivalentTo(_risks);
    }

    [Fact]
    public void Policy_CreateNewPolicy_CheckValidTillTime()
    {
        _homePolicy.ValidTill.Should().Be(new DateTime(2024, 8, 1));
    }

    [Fact]
    public void Policy_CreateNewPolicy_ReturnValidPremium()
    {
        decimal expectedPremium = 900;

        _homePolicy.Premium.Should().Be(_risks.Aggregate(0M, (a, x) => a += x.YearlyPrice) / 2);
        _homePolicy.Premium.Should().Be(expectedPremium);
    }

    [Fact]
    public void Policy_AddNewRisk_ReturnCorrectPremium()
    {
        var zombieRisk = new Risk("Zombie", 200);
        _homePolicy.AddRisk(zombieRisk, _startDate);

        decimal expectedPremium = 1000;

        _homePolicy.Premium.Should()
            .Be((_risks.Aggregate((decimal)0, (a, x) => a += x.YearlyPrice) + zombieRisk.YearlyPrice) / 2);

        _homePolicy.Premium.Should().Be(expectedPremium);
    }

    [Fact]
    public void Policy_RemoveRisk_RiskIsRemovedAndPremiumIsRecalculated()
    {
        var isRiskRemoved = _homePolicy.RemoveRisk("Aliens");
        var expectedPremium = 300;
        var expectedRiskCount = 3;


        isRiskRemoved.Should().BeTrue();
        _homePolicy.InsuredRisks.Count.Should().Be(expectedRiskCount);
        _homePolicy.Premium.Should().Be(expectedPremium);
    }

    [Fact]
    public void Policy_RemoveRisk_RiskIsNotRemoved()
    {
        var isRiskRemoved = _homePolicy.RemoveRisk("Sharks");
        var expectedRiskCount = 4;

        isRiskRemoved.Should().BeFalse();
        _homePolicy.InsuredRisks.Count.Should().Be(expectedRiskCount);
    }
}
