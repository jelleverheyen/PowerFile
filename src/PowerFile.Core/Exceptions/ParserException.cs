namespace PowerFile.Core.Exceptions;

public class ParserException : Exception
{
    public ParserException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
    public ParserException(string message) : base(message)
    {
    }
}