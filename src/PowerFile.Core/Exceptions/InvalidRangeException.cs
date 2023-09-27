namespace PowerFile.Core.Exceptions;

public class InvalidRangeException(string message, string content) : ParserException($"Invalid range specified '{content}': {message}")
{
    
}