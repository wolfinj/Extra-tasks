using FluentAssertions;
using If_Risk.Exceptions;

namespace If_Risk.UnitTests;

public class InsuranceCompanyTest
{
    private readonly InsuranceCompany _megaSafe;
    private readonly List<Risk> _risksForPolicy;
    private readonly List<Risk> _availableRisks;

    private readonly DateTime _startDate;
    private readonly DateTime _startDateInProgress;
    private readonly DateTime _startDateAfterExpiring;

    private readonly Risk _validTestRisk;
    private readonly Risk _invalidTestRisk;

    public InsuranceCompanyTest()
    {
        _startDate = new(2024, 02, 01);
        _startDateInProgress = new(2024, 04, 01);
        _startDateAfterExpiring = new(2026, 04, 01);

        _validTestRisk = new Risk("Theft", 200);
        _invalidTestRisk = new Risk("Giants", 200);

        _availableRisks = new List<Risk>
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


        _megaSafe = new InsuranceCompany("Mega Safe Insurance", _availableRisks);
    }

    [Fact]
    public void InsuranceCompany_CreateNewCompany_CheckName()
    {
        _megaSafe.Name.Should().Be("Mega Safe Insurance");
    }

    [Fact]
    public void InsuranceCompany_CreateNewCompany_CompanyHas5Risks()
    {
        _megaSafe.AvailableRisks.Count.Should().Be(_availableRisks.Count);
    }

    [Fact]
    public void InsuranceCompany_SellPolicy_PolicyAddedToPoliciesList()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);
        _megaSafe.Policies.Count.Should().Be(1);
    }

    [Fact]
    public void InsuranceCompany_SellPolicyInPast_ThrowsInvalidPolicyStartingTimeException()
    {
        InvalidPolicyStartingTimeException exception = Assert.Throws<InvalidPolicyStartingTimeException>(() =>
            _megaSafe.SellPolicy(
                "Home insurance",
                DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                8,
                _risksForPolicy
            ));

        Assert.Equal("Policy starting date can not be in past!", exception.Message);
    }

    [Fact]
    public void InsuranceCompany_SellPolicyWithOverlappingDates_ThrowsPolicyPeriodOverlapException()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);


        PolicyPeriodOverlapException exception = Assert.Throws<PolicyPeriodOverlapException>(() =>
            _megaSafe.SellPolicy(
                "Home insurance",
                _startDate,
                8,
                _risksForPolicy
            ));

        Assert.Equal("Policy with the same name can't overlap period!", exception.Message);
    }

    [Fact]
    public void InsuranceCompany_SellPolicyWithUnavailableRisks_ThrowsRiskNotAvailableException()
    {
        var risksForPolicyWrong = new List<Risk>
        {
            new("Swap monster", 9999),
            new("Aliens", 1200),
            new("Ants", 100),
            new("Zombie", 200)
        };

        var act = () => _megaSafe.SellPolicy("Home insurance", _startDate, 8, risksForPolicyWrong);

        act.Should().Throw<RiskNotAvailableException>().WithMessage("One or more risks are not available!");

        RiskNotAvailableException exception = Assert.Throws<RiskNotAvailableException>(() =>
            _megaSafe.SellPolicy(
                "Home insurance",
                _startDate,
                8,
                risksForPolicyWrong
            ));

        Assert.Equal("One or more risks are not available!", exception.Message);
    }


    [Fact]
    public void InsuranceCompany_AddRiskToNonExistingPolicy_ThrowsPolicyNotFoundException()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);

        PolicyNotFoundException exception = Assert.Throws<PolicyNotFoundException>(() =>
            _megaSafe.AddRisk("Ship insurance", _validTestRisk, _startDateInProgress)
        );

        Assert.Equal("Insurance \"Ship insurance\" do not exist!", exception.Message);
    }

    [Fact]
    public void InsuranceCompany_AddUnavailableRisk_ThrowsRiskDoesNotExistsException()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);

        RiskDoesNotExistsException exception = Assert.Throws<RiskDoesNotExistsException>(() =>
            _megaSafe.AddRisk("Ship insurance", _invalidTestRisk, _startDateInProgress)
        );

        Assert.Equal("Giants is not available!", exception.Message);
    }

    [Fact]
    public void InsuranceCompany_AddRiskAfterPolicyExpired_ThrowsPolicyExpiredException()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);

        PolicyExpiredException exception = Assert.Throws<PolicyExpiredException>(() =>
            _megaSafe.AddRisk("Home insurance", _validTestRisk, _startDateAfterExpiring)
        );

        Assert.Equal($"Policy expired at {_startDate.AddMonths(8)}", exception.Message);
    }

    [Fact]
    public void InsuranceCompany_AddRiskToActivePolicy_RiskCountIncreasedPremiumRecalculated()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);

        _megaSafe.AddRisk("Home insurance", _validTestRisk, _startDateInProgress);

        _megaSafe.Policies.First().InsuredRisks.Count.Should().Be(4);
        _megaSafe.Policies.First().Premium.Should().Be(1100);
    }


    [Fact]
    public void InsuranceCompany_ReturnWrongInsuranceName_ThrowsInsuredObjectDoesNotExistsException()
    {
        InsuredObjectDoesNotExistsException exception = Assert.Throws<InsuredObjectDoesNotExistsException>(() =>
            _megaSafe.GetPolicy("Home insurance", _startDateInProgress)
        );

        Assert.Equal("There is no policy with name \"Home insurance\"!", exception.Message);
    }


    [Fact]
    public void InsuranceCompany_InputWrongDate_ThrowsInvalidPolicyException()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);

        InvalidPolicyException exception = Assert.Throws<InvalidPolicyException>(() =>
            _megaSafe.GetPolicy("Home insurance", _startDateAfterExpiring)
        );

        Assert.Equal("There is no valid Policy at this time!", exception.Message);
    }


    [Fact]
    public void InsuranceCompany_GetPolicy_ReturnsCorrectPolicy()
    {
        _megaSafe.SellPolicy("Home insurance", _startDate, 8, _risksForPolicy);

        Policy policy = new Policy("Home insurance", _startDate, _startDate.AddMonths(8), _risksForPolicy);

        _megaSafe.GetPolicy("Home insurance", _startDateInProgress).Should().BeEquivalentTo(policy);
    }
}
