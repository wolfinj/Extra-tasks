namespace If_Risk;

public class InsuranceCompany :IInsuranceCompany
{
    public string Name { get; }
    
    public IList<Risk> AvailableRisks { get; set; }
    
    
    public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
    {
        throw new NotImplementedException();
    }

    public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
    {
        throw new NotImplementedException();
    }

    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        throw new NotImplementedException();
    }
}