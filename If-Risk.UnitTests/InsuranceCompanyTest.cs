using FluentAssertions;

namespace If_Risk.UnitTests;

public class InsuranceCompanyTest
{
    private readonly InsuranceCompany _megaSafe;
    private readonly List<Risk> _risksForPolicy;

    private readonly DateTime _startDate;
    private DateTime _startDateInProgress;
    private DateTime _startDateAfterExpiring;
    
    private Risk validTestRisk;
    private Risk invalidTestRisk;

    public InsuranceCompanyTest()
    {
        _startDate = new(2024, 02, 01);
        _startDateInProgress = new(2024, 04, 01);
        _startDateAfterExpiring = new(2026, 04, 01);

        validTestRisk = new Risk("Theft", 200);
        invalidTestRisk = new Risk("Giants", 200);

        var risks = new List<Risk>
        {
            new("Theft", 200),
            new("Flood", 300),
            new("Aliens", 1200),
            new("Ants", 100),
            new("Zombie", 200)
        };

        _risksForPolicy = new List<Risk>
        {
            new("Aliens", 1200),
            new("Ants", 100),
            new("Zombie", 200)
        };


        _megaSafe = new InsuranceCompany("Mega Safe Insurance", risks);
    }

    [Fact]
    public void InsuranceCompany_CreateNewCompany_CheckName()
    {
        _megaSafe.Name.Should().Be("Mega Safe Insurance");
    }

    [Fact]
    public void InsuranceCompany_CreateNewCompany_CheckAvailableRisks()
    {
        _megaSafe.AvailableRisks.Count.Should().Be(5);
    }

    [Fact]
    public void InsuranceCompany_SellPolicy_CheckPolicyCount()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);
        _megaSafe.Policies.Count.Should().Be(1);
    }

    [Fact]
    public void InsuranceCompany_SellPolicyInPast_GetException()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            _megaSafe.SellPolicy(
                "Home insurance",
                DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                8,
                _risksForPolicy
            ));

        Assert.Equal("Policy starting date can not be in past!", exception.Message);
    }

    [Fact]
    public void InsuranceCompany_SellPolicyWithOverlappingDates_GetException()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);


        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            _megaSafe.SellPolicy(
                "Home insurance",
                _startDate,
                8,
                _risksForPolicy
            ));

        Assert.Equal("Policy with the same name cant overlap period!", exception.Message);
    }

    [Fact]
    public void InsuranceCompany_SellPolicyWithUnavailableRisks_GetException()
    {
        var risksForPolicyWrong = new List<Risk>
        {
            new("Swap monster", 9999),
            new("Aliens", 1200),
            new("Ants", 100),
            new("Zombie", 200)
        };

        // _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicyWrong).Should().Throw<ArgumentException>().WithMessage("Hello is not allowed at this moment");
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            _megaSafe.SellPolicy(
                "Home insurance",
                _startDate,
                8,
                risksForPolicyWrong
            ));

        Assert.Equal("One or more risks are not available!", exception.Message);
    }


    [Fact]
    public void InsuranceCompany_AddRiskToNonExistingPolicy_GetException()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);

        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            _megaSafe.AddRisk("Ship insurance", validTestRisk, _startDateInProgress)
        );

        Assert.Equal("Insurance \"Ship insurance\" do not exist!", exception.Message);
    }

    [Fact]
    public void InsuranceCompany_AddRiskAfterPolicyExpired_GetException()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);

        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            _megaSafe.AddRisk("Home insurance", validTestRisk, _startDateAfterExpiring)
        );

        Assert.Equal($"Policy expired at {_startDate.AddMonths(8)}", exception.Message);
    }

    [Fact]
    public void InsuranceCompany_AddRiskToActivePolicy_GetRiskCountAndCorrectPremium()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);

        _megaSafe.AddRisk("Home insurance", validTestRisk, _startDateInProgress);

        _megaSafe.Policies.First().InsuredRisks.Count.Should().Be(4);
        _megaSafe.Policies.First().Premium.Should().Be(1100);
    }
}
