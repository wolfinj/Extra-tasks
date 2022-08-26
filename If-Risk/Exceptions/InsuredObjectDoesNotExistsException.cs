namespace If_Risk.Exceptions;

public class InsuredObjectDoesNotExistsException: Exception
{
    public InsuredObjectDoesNotExistsException() :base("There is no policy with this name!")
    {
    }

    public InsuredObjectDoesNotExistsException(string message)
        : base(message)
    {
    }

    
}
