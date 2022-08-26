namespace If_Risk.Exceptions;

public class InvalidPolicyStartingTimeException: Exception
{
    public InvalidPolicyStartingTimeException() :base("Policy starting date can not be in past!")
    {
    }

    public InvalidPolicyStartingTimeException(string message)
        : base(message)
    {
    }

    
}
