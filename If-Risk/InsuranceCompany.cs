namespace If_Risk;

public class InsuranceCompany :IInsuranceCompany
{
    /// <summary>
    /// Name of Insurance company
    /// </summary>
    private readonly string _name;

    private List<Risk> _availableRisks;

    public string Name => _name;

    /// <summary>
    /// List of the risks that can be insured. List can be updated at any time
    /// </summary>
    public IList<Risk> AvailableRisks
    {
        get => _availableRisks;
        set => _availableRisks = (List<Risk>)value;
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
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add risk to the policy of insured object.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="risk">Risk that must be added</param>
    /// <param name="validFrom">Date when risk becomes active. Can not be in the past</param>
    public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
    {
        AvailableRisks.Add(new Risk());
    }

    /// <summary>
    /// Gets policy with a risks at the given point of time.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="effectiveDate">Point of date and time, when the policy effective</param>
    /// <returns></returns>
    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        throw new NotImplementedException();
    }
}
