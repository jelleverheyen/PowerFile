namespace PowerFile.Core.Scanning;

public class Token(TokenType type, string lexeme, object? literal)
{
    public TokenType Type { get; } = type;
    public string Lexeme { get; } = lexeme;
    public object? Literal { get; } = literal;

    public override string ToString()
    {
        return $"{type} - '{lexeme}'";
    }
}