namespace If_Risk.Exceptions;

public class RiskNotAvailableException: Exception
{
    public RiskNotAvailableException() :base("One or more risks are not available!")
    {
    }

    public RiskNotAvailableException(string message)
        : base(message)
    {
    }

    
}
