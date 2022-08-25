namespace If_Risk.Exceptions;

public class PolicyExpiredException: Exception
{
    public PolicyExpiredException() :base("Policy expired!")
    {
    }

    public PolicyExpiredException(string message)
        : base(message)
    {
    }

    
}
