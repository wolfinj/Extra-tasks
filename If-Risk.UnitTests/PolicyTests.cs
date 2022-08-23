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
        _homePolicy.Premium.Should().Be(_risks.Aggregate(0M, (a, x) => a += x.YearlyPrice) / 2);
        _homePolicy.Premium.Should().Be(900);
    }

    [Fact]
    public void Policy_AddNewRisk_ReturnCorrectPremium()
    {
        var zombieRisk = new Risk("Zombie", 200);
        _homePolicy.AddRisk(zombieRisk);

        _homePolicy.Premium.Should().Be(_risks.Aggregate(0M, (a, x) => a += x.YearlyPrice) / 2);
        _homePolicy.Premium.Should().Be(1000);
    }

    [Fact]
    public void Policy_RemoveRisk_RiskIsRemovedAndPremiumIsRecalculated()
    {
        var isRiskRemoved = _homePolicy.RemoveRisk("Aliens");

        isRiskRemoved.Should().BeTrue();
        _homePolicy.InsuredRisks.Count.Should().Be(3);
        _homePolicy.Premium.Should().Be(300);
    }

    [Fact]
    public void Policy_RemoveRisk_RiskIsNotRemoved()
    {
        var isRiskRemoved = _homePolicy.RemoveRisk("Sharks");

        isRiskRemoved.Should().BeFalse();
        _homePolicy.InsuredRisks.Count.Should().Be(4);
        _homePolicy.Premium.Should().Be(900);
    }
}
