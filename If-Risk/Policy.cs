using If_Risk.Exceptions;

namespace If_Risk;

public class Policy : IPolicy
{
    /// <summary>
    ///     Initially included risks or risks at specific moment of time.
    /// </summary>
    private readonly Dictionary<Risk,DateTime> _insuredRisks;

    /// <summary>
    ///     Date when policy becomes inactive
    /// </summary>
    private readonly DateTime _validTill;
    
    /// <summary>
    ///     Date when policy becomes active
    /// </summary>
    private readonly DateTime _validFrom;
    
    /// <summary>
    ///     Total months policy is valid
    /// </summary>
    private readonly int _monthValid;

    private readonly string _nameOfInsuredObject;

    public Policy(string nameOfInsuredObject, DateTime validFrom, DateTime validTill, List<Risk> insuredRisks)
    {
        _monthValid = validTill.Month - validFrom.Month;
        _nameOfInsuredObject = nameOfInsuredObject;
        _validFrom = validFrom;
        _validTill = validTill;
        _insuredRisks = insuredRisks.ToDictionary(x => x,x=> validFrom);
    }


    /// <summary>
    ///     Name of insured object
    /// </summary>
    public string NameOfInsuredObject => _nameOfInsuredObject;

    public DateTime ValidFrom => _validFrom;

    public DateTime ValidTill => _validTill;

    /// <summary>
    ///     Total price of the policy. Calculate by summing up all insured risks.
    ///     Take into account that risk price is given for 1 full year. Policy/risk period can be shorter.
    /// </summary>
    public decimal Premium => CalculatePremium(_insuredRisks, _validTill);

    public IList<Risk> InsuredRisks
    {
        get { return _insuredRisks.Select(x => x.Key).ToList(); }
    }

    public void AddRisk(Risk risk,DateTime validFrom)
    {
        _insuredRisks.Add(risk,validFrom);
    }

    public bool RemoveRisk(string name)
    {
        Risk toRemove = _insuredRisks.FirstOrDefault(x => x.Key.Name == name).Key;
        
        return _insuredRisks.Remove(toRemove);
    }

    private decimal CalculatePremium(Dictionary<Risk,DateTime> risks, DateTime policyValidTill)
    {
        decimal premium = 0;
        foreach (var risk in risks)
        {
            var duration = policyValidTill.Month - risk.Value.Month;
            premium += risk.Key.YearlyPrice / 12 * duration;
        }
        return premium;
    }
}
