namespace PowerFile.Core.Exceptions;

public class InvalidGroupException(string message) : ParserException($"Invalid group usage: {message}")
{
}