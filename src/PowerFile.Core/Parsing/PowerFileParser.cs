using PowerFile.Core.Exceptions;
using PowerFile.Core.Expressions;
using PowerFile.Core.Extensions;
using PowerFile.Core.Scanning;

namespace PowerFile.Core.Parsing;

public class PowerFileParser(IEnumerable<Token> tokens)
{
    private int _current;
    private readonly Stack<GroupExpression> _groupStack = new();
    private readonly Token[] _tokens = tokens.ToArray();

    public PowerFileExpression Parse()
    {
        StartExplicitGroup();
        
        while (_groupStack.Any())
        {
            if (Check(TokenType.String) || Check(TokenType.DirectorySeparator))
            {
                AddToCurrentGroup(new TextExpression(Advance().Lexeme));
                continue;
            }

            if (Peek().IsExplicitGroupCharacter())
                IsValidGroupToken();

            if (Check(TokenType.LeftBracket))
                ParseRangeExpression();

            if (Check(TokenType.LeftParenthesis))
                StartExplicitGroup();
            
            if (Peek().IsGroupTerminator())
            {
                var group = _groupStack.Pop();
                if (!_groupStack.Any())
                    return group;

                AddToCurrentGroup(group);
                if (Check(TokenType.RightParenthesis))
                    AddToCurrentGroup(_groupStack.Pop());
            }

            if (Check(TokenType.Comma))
                _groupStack.Push(new ExpandableGroupExpression());

            Advance();
        }

        throw new ParserException("Failed to parse the pattern for an unknown reason");
    }

    private void ParseRangeExpression()
    {
        if (Check(TokenType.LeftBracket))
            Advance();

        object? left = null;
        if (IsValidRangeMember())
            left = Advance().Literal;

        if (Check(TokenType.Comma))
            Advance();
        
        object? right = null;
        if (IsValidRangeMember())
            right = Advance().Literal;

        if (left is not string l || right is not string r)
            throw new InvalidRangeException("One or more range members could not be parsed", string.Empty);

        if (Check(TokenType.RightBracket))
        {
            AddToCurrentGroup(new RangeExpression(l, r));
            return;
        }

        throw new InvalidRangeException("Invalid range members", $"Left: {left} Right: {right}");
    }

    private bool IsValidRangeMember()
    {
        return Check(TokenType.String) && Peek().Literal is not null;
    }

    private void AddToCurrentGroup(PowerFileExpression expression)
    {
        if (!_groupStack.Any())
            throw new InvalidGroupException($"Attempted to close non-existing group with '{Peek().Lexeme}'");
            
        _groupStack.Peek().AddChild(expression);
    }

    private bool Check(TokenType type)
    {
        return Peek().Type == type;
    }

    private Token Advance()
    {
        if (!IsAtEnd()) _current++;
        return Previous();
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EndOfFile;
    }

    private Token Peek()
    {
        return _tokens[_current];
    }

    private Token Previous()
    {
        return _tokens[_current - 1];
    }

    private void StartExplicitGroup()
    {
        _groupStack.Push(new TextGroupExpression());
        _groupStack.Push(new ExpandableGroupExpression());
    }

    private bool IsValidGroupToken()
    {
        var current = Peek();
        var hasGroup = _groupStack.TryPeek(out var group);
        if (!hasGroup && current.IsExplicitGroupTerminator())
            throw new InvalidGroupException($"Attempting to close non-existing group with '{current.Literal}'");

        switch (group)
        {
            case ExpandableGroupExpression or TextGroupExpression when current.Type is TokenType.LeftParenthesis or TokenType.RightParenthesis or TokenType.LeftBracket:
            case RangeExpression when current.Type is TokenType.RightBracket:
                return true;
            default:
                throw new InvalidGroupException($"Invalid group character '{current.Lexeme}'");
        }
    }
}