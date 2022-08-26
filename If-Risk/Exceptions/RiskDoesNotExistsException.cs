namespace If_Risk.Exceptions;

public class RiskDoesNotExistsException : Exception
{
    public RiskDoesNotExistsException() :base("Risk is not available!")
    {
    }

    public RiskDoesNotExistsException(string message)
        : base(message)
    {
    }

    
}
