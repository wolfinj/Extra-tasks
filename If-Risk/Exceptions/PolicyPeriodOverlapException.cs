namespace If_Risk.Exceptions;

public class PolicyPeriodOverlapException: Exception
{
    public PolicyPeriodOverlapException() :base("Policy with the same name can't overlap period!")
    {
    }

    public PolicyPeriodOverlapException(string message)
        : base(message)
    {
    }

    
}
