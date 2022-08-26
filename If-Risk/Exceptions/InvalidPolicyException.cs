namespace If_Risk.Exceptions;

public class InvalidPolicyException: Exception
{
    public InvalidPolicyException() :base("There is no valid Policy at this time!")
    {
    }

    public InvalidPolicyException(string message)
        : base(message)
    {
    }

    
}
