using If_Risk.Exceptions;

namespace If_Risk;

public class InsuranceCompany : IInsuranceCompany
{
    /// <summary>
    ///     List of sold policies
    /// </summary>
    public readonly List<Policy> Policies;

    /// <summary>
    ///     List of the risks that can be insured. List can be updated at any time
    /// </summary>
    private List<Risk> _availableRisks;

    public InsuranceCompany(string name, List<Risk> availableRisks)
    {
        Name = name;
        _availableRisks = availableRisks;
        Policies = new List<Policy>();
    }

    public InsuranceCompany(string name, List<Risk> availableRisks, List<Policy> startingPolicies)
    {
        Name = name;
        _availableRisks = availableRisks;
        Policies = startingPolicies;
    }

    /// <summary>
    ///     Name of Insurance company
    /// </summary>
    public string Name { get; }

    public IList<Risk> AvailableRisks
    {
        get => _availableRisks;
        set => _availableRisks = (List<Risk>)value;
    }

    /// <summary>
    ///     Sell the policy.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of the insured object. Must be unique in the given period.</param>
    /// <param name="validFrom">Date and time when policy starts. Can not be in the past</param>
    /// <param name="validMonths">Policy period in months</param>
    /// <param name="selectedRisks">List of risks that must be included in the policy</param>
    /// <returns>Information about policy</returns>
    public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths,
        List<Risk> selectedRisks)
    {
        ValidateInputs(nameOfInsuredObject, validFrom, validMonths, selectedRisks);

        var newPolicy = new Policy(nameOfInsuredObject, validFrom, validFrom.AddMonths(validMonths), selectedRisks);

        Policies.Add(newPolicy);

        return newPolicy;
    }

    private void ValidateInputs(string nameOfInsuredObject, DateTime validFrom, short validMonths,
        IList<Risk> selectedRisks)
    {
        if (validFrom < DateTime.Now) throw new InvalidPolicyStartingTimeException();

        if (Policies.Exists(x => x.NameOfInsuredObject == nameOfInsuredObject) &&
            IsDatesOverlapping(
                validFrom,
                validFrom.AddMonths(validMonths),
                FindInsuredObjectByName(nameOfInsuredObject)!.ValidFrom,
                FindInsuredObjectByName(nameOfInsuredObject)!.ValidTill
            ))
            throw new PolicyPeriodOverlapException();

        if (selectedRisks.Intersect(_availableRisks).Count() != selectedRisks.Count)
            throw new RiskNotAvailableException();
    }

    /// <summary>
    ///     Add risk to the policy of insured object.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="risk">Risk that must be added</param>
    /// <param name="validFrom">Date when risk becomes active. Can not be in the past</param>
    public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
    {
        var selectedInsuranceObject = FindInsuredObjectByName(nameOfInsuredObject);

        ValidateAddRisk(nameOfInsuredObject, risk, validFrom, selectedInsuranceObject);

        selectedInsuranceObject.AddRisk(risk, validFrom);
    }

    private void ValidateAddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom,
        Policy? selectedInsuranceObject)
    {
        if (!_availableRisks.Contains(risk))
        {
            throw new RiskDoesNotExistsException($"{risk.Name} is not available!");
        }

        if (selectedInsuranceObject == null)
        {
            throw new PolicyNotFoundException($"Insurance \"{nameOfInsuredObject}\" do not exist!");
        }

        if (validFrom > selectedInsuranceObject.ValidTill)
        {
            throw new PolicyExpiredException($"Policy expired at {selectedInsuranceObject.ValidTill}");
        }
    }

    private Policy? FindInsuredObjectByName(string nameOfInsuredObject)
    {
        return Policies.Find(x => x.NameOfInsuredObject == nameOfInsuredObject);
    }

    /// <summary>
    ///     Gets policy with a risks at the given point of time.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="effectiveDate">Point of date and time, when the policy effective</param>
    /// <returns></returns>
    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        if (!Policies.Exists(x => x.NameOfInsuredObject == nameOfInsuredObject))
            throw new InsuredObjectDoesNotExistsException($"There is no policy with name \"{nameOfInsuredObject}\"!");

        return Policies.Find(x =>
            x.NameOfInsuredObject == nameOfInsuredObject && x.ValidFrom <= effectiveDate &&
            x.ValidTill >= effectiveDate) ?? throw new InvalidPolicyException();
    }

    private bool IsDatesOverlapping(DateTime aTimeStart, DateTime aTimeEnd, DateTime bTimeStart, DateTime bTimeEnd)
    {
        return aTimeStart < bTimeEnd && aTimeEnd > bTimeStart;
    }
}
