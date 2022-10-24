namespace Gryzilla_App.Exceptions;

public class SameNameException: Exception
{
    public SameNameException() {}

    public SameNameException(string message): base(message) {}

}