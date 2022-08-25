namespace If_Risk.Exceptions;

public class PolicyNotFoundException: Exception
{
    public PolicyNotFoundException() :base("Insurance do not exist!")
    {
    }

    public PolicyNotFoundException(string message)
        : base(message)
    {
    }

    
}
