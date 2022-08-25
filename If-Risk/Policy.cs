namespace If_Risk;

public class Policy : IPolicy
{
    /// <summary>
    ///     Initially included risks or risks at specific moment of time.
    /// </summary>
    private readonly List<Risk> _insuredRisks;

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
    private decimal _premium;

    public Policy(string nameOfInsuredObject, DateTime validFrom, DateTime validTill, List<Risk> insuredRisks)
    {
        _monthValid = validTill.Month - validFrom.Month;
        _nameOfInsuredObject = nameOfInsuredObject;
        _validFrom = validFrom;
        _validTill = validTill;
        _insuredRisks = insuredRisks;
        CalculatePremium();
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
    public decimal Premium
    {
        get => _premium;
        private set => _premium = value;
    }

    public IList<Risk> InsuredRisks => _insuredRisks;

    public void AddRisk(Risk risk)
    {
        _insuredRisks.Add(risk);
        CalculatePremium();
    }

    public void AddRiskAfterSelling(Risk risk, DateTime validFrom)
    {
        _insuredRisks.Add(risk);
        Premium += risk.YearlyPrice / 12 * (_validTill.Month - validFrom.Month);
    }


    public bool RemoveRisk(string name)
    {
        var index = _insuredRisks.FindIndex(x => x.Name == name);
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
        Premium = _insuredRisks.Aggregate((decimal)0, (a, x) => a += x.YearlyPrice) / 12 * _monthValid;
    }
}
