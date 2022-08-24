namespace If_Risk;

public class InsuranceCompany :IInsuranceCompany
{
    /// <summary>
    /// Name of Insurance company
    /// </summary>
    private readonly string _name;

    /// <summary>
    /// List of the risks that can be insured. List can be updated at any time
    /// </summary>
    private List<Risk> _availableRisks;

    /// <summary>
    /// List of sold policies
    /// </summary>
    public readonly List<Policy> Policies;

    public string Name => _name;

    public IList<Risk> AvailableRisks
    {
        get => _availableRisks;
        set => _availableRisks = (List<Risk>)value;
    }

    public InsuranceCompany(string name, List<Risk> availableRisks)
    {
        _name = name;
        _availableRisks = availableRisks;
        Policies = new List<Policy>();
    }

    /// <summary>
    /// Sell the policy.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of the insured object. Must be unique in the given period.</param>
    /// <param name="validFrom">Date and time when policy starts. Can not be in the past</param>
    /// <param name="validMonths">Policy period in months</param>
    /// <param name="selectedRisks">List of risks that must be included in the policy</param>
    /// <returns>Information about policy</returns>
    public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
    {
        if (validFrom < DateTime.Now)
        {
            throw new ArgumentException("Policy starting date can not be in past!");
        }

        if (Policies.Exists(x => x.NameOfInsuredObject == nameOfInsuredObject) &&
            IsDatesOverlapping(
                validFrom,
                validFrom.AddMonths(validMonths),
                Policies.Find(x => x.NameOfInsuredObject == nameOfInsuredObject)!.ValidFrom,
                Policies.Find(x => x.NameOfInsuredObject == nameOfInsuredObject)!.ValidTill
            ))
        {
            throw new ArgumentException("Policy with the same name cant overlap period!");
        }

        if (selectedRisks.Intersect(_availableRisks).Count() != selectedRisks.Count)
        {
            throw new ArgumentException("One or more risks are not available!");
        }
        
        var newPolicy = new Policy(nameOfInsuredObject, validFrom, validFrom.AddMonths(validMonths), selectedRisks);
        
        Policies.Add(newPolicy);
        
        return newPolicy;
    }

    /// <summary>
    /// Add risk to the policy of insured object.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="risk">Risk that must be added</param>
    /// <param name="validFrom">Date when risk becomes active. Can not be in the past</param>
    public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
    {
        
        var selectedInsuranceObject = Policies.Find(x => x.NameOfInsuredObject == nameOfInsuredObject);
        if (selectedInsuranceObject == null)
        {
            throw new ArgumentException($"Insurance \"{nameOfInsuredObject}\" do not exist!");
        }

        if (validFrom > selectedInsuranceObject.ValidTill)
        {
            throw new ArgumentException($"Policy expired at {selectedInsuranceObject.ValidTill}");
        }
        
        selectedInsuranceObject.AddRiskAfterSelling(risk,validFrom);
    }

    /// <summary>
    /// Gets policy with a risks at the given point of time.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="effectiveDate">Point of date and time, when the policy effective</param>
    /// <returns></returns>
    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        //todo
        throw new NotImplementedException();
    }

    private bool IsDatesOverlapping(DateTime a1, DateTime a2, DateTime b1, DateTime b2)
    {
        if (a1 < b2 && a2 > b1) return true;

        return false;
    }
}
