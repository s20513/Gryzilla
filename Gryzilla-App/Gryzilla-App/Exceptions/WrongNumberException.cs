namespace Gryzilla_App.Exceptions;

public class WrongNumberException : Exception
{
    public WrongNumberException() {}
    
    public WrongNumberException(string message) : base(message) {}
}