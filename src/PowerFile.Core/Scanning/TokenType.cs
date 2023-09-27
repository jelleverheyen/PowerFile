namespace PowerFile.Core.Scanning;

public enum TokenType
{
    LeftParenthesis, RightParenthesis,
    LeftBracket, RightBracket,
    String,
    DirectorySeparator,
    Comma,
    EndOfFile,
}