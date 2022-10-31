namespace Gryzilla_App.Exceptions;

public class UserCreatorException : Exception
{
    public UserCreatorException() {}
    
    public UserCreatorException(string message) : base(message) {}
}