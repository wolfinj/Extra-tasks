using FluentAssertions;

namespace If_Risk.UnitTests;

public class InsuranceCompanyTest
{
    private readonly InsuranceCompany _megaSafe;
    private readonly List<Risk> _risksForPolicy;

    private readonly DateTime _startDate;

    public InsuranceCompanyTest()
    {
        _startDate = new(2024, 02, 01);

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
}
