namespace PowerFile.Core.Scanning;

public class Scanner(string source)
{
    private int _start;
    private int _current;
    private readonly List<Token> _tokens = new();

    public IEnumerable<Token> Scan()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.EndOfFile, "", null));

        return _tokens;
    }

    private bool IsAtEnd()
    {
        return _current >= source.Length;
    }

    /// <summary>
    /// Moves the scanner to the next character
    /// </summary>
    /// <returns>The character before incrementing</returns>
    private char Advance()
    {
        return source[_current++];
    }

    private void AddToken(TokenType type, object? literal = null)
    {
        var text = source[_start.._current];
        _tokens.Add(new Token(type, text, literal));
    }

    /// <summary>
    /// Checks the current token and adds the appropriate token
    /// </summary>
    private void ScanToken()
    {
        var c = Advance();
        switch (c)
        {
            case '(':
                AddToken(TokenType.LeftParenthesis);
                break;
            case ')':
                AddToken(TokenType.RightParenthesis);
                break;
            case '[':
                AddToken(TokenType.LeftBracket);
                break;
            case ']':
                AddToken(TokenType.RightBracket);
                break;
            case ',':
                AddToken(TokenType.Comma);
                break;
            // case '/':
            //     AddToken(TokenType.DirectorySeparator);
            //     break;
            // case '\\':
            //     AddToken(TokenType.DirectorySeparator);
            //     break;
            default:
                ScanString();
                break;
        }
    }
    /// <summary>
    /// Runs through a string until the end and adds a token
    /// </summary>
    private void ScanString()
    {
        while (IsValidPathCharacter(Peek()) && !IsAtEnd())
            Advance();
        
        AddToken(TokenType.String, source[_start.._current]);
    }

    /// <summary>
    /// Current character in the string
    /// </summary>
    private char Peek()
    {
        return IsAtEnd()
            ? '\u0000'
            : source[_current];
    }

    private char PeekNext()
    {
        return _current + 1 >= source.Length
            ? '\u0000'
            : source[_current + 1];
    }

    private bool Match(char expected)
    {
        if (IsAtEnd())
            return false;

        if (source[_current] != expected)
            return false;

        _current++;

        return true;
    }

    private static bool IsValidPathCharacter(char c)
    {
        return char.IsDigit(c)
               || char.IsBetween(char.ToLower(c), 'a', 'z')
               || c == '_'
               || c == '-'
               || c == '.'
               || c == ' '
               || c == '/'
               || c == '\\';
    }
}