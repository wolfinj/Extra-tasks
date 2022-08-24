namespace If_Risk;

public class Policy : IPolicy
{
    /// <summary>
    /// Name of insured object
    /// </summary>
    private readonly string _nameOfInsuredObject;

    /// <summary>
    /// Date when policy becomes active
    /// </summary>
    private readonly DateTime _validFrom;

    /// <summary>
    /// Date when policy becomes inactive
    /// </summary>
    private readonly DateTime _validTill;

    /// <summary>
    /// Total price of the policy. Calculate by summing up all insured risks.
    /// Take into account that risk price is given for 1 full year. Policy/risk period can be shorter.
    /// </summary>
    private decimal _premium;

    /// <summary>
    /// Total months policy is valid
    /// </summary>
    private int _monthValid;

    /// <summary>
    /// Initially included risks or risks at specific moment of time.
    /// </summary>
    private readonly List<Risk> _insuredRisks;


    public string NameOfInsuredObject => _nameOfInsuredObject;

    public DateTime ValidFrom => _validFrom;

    public DateTime ValidTill => _validTill;

    public decimal Premium => _premium;

    public IList<Risk> InsuredRisks => _insuredRisks;

    public Policy(string nameOfInsuredObject, DateTime validFrom, DateTime validTill, IList<Risk> insuredRisks)
    {
        _monthValid = validTill.Month - validFrom.Month;
        _nameOfInsuredObject = nameOfInsuredObject;
        _validFrom = validFrom;
        _validTill = validTill;
        _insuredRisks = (List<Risk>?)insuredRisks;
        CalculatePremium();
    }

    public void AddRisk(Risk risk)
    {
        _insuredRisks.Add(risk);
        CalculatePremium();
    }
    
    public void AddRiskAfterSelling(Risk risk, DateTime validFrom)
    {
        _insuredRisks.Add(risk);
        _premium += risk.YearlyPrice / 12 * (_validTill.Month - validFrom.Month);
    }


    public bool RemoveRisk(string name)
    {
        int index = _insuredRisks.FindIndex(x => x.Name == name);
        if (index is not -1)
        {
            _insuredRisks.RemoveAt(index);
            CalculatePremium();
            return true;
        }

        return false;
    }

    private void CalculatePremium()
    {
        _premium = _insuredRisks.Aggregate((decimal)0, (a, x) => a += x.YearlyPrice) / 12 * _monthValid;
    }
}
