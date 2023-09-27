using PowerFile.Core.Parsing;
using PowerFile.Core.Scanning;

namespace PowerFile.Core.Extensions;

public static class TokenExtensions
{
    /// <summary>
    /// Check if the <see cref="Token"/> is an explicit group opener '(' or '['
    /// </summary>
    /// <param name="token">Token to be checked</param>
    /// <returns>True if the <see cref="Token"/> is a group an explicit group opener</returns>
    public static bool IsExplicitGroupOpener(this Token token) => token.Type is TokenType.LeftParenthesis or TokenType.LeftBracket;
    
    /// <summary>
    /// Check if the <see cref="Token"/> is an explicit group closer ')' or ']'
    /// </summary>
    /// <param name="token">Token to be checked</param>
    /// <returns>True if the token is a group an explicit group closer</returns>
    public static bool IsExplicitGroupTerminator(this Token token) => token.Type is TokenType.RightParenthesis or TokenType.RightBracket;
    
    /// <summary>
    /// Check if the <see cref="Token"/> is an explicit group character: ( ) or [ ]
    /// </summary>
    /// <param name="token">Token to be checked</param>
    /// <returns>True if the token is a group an explicit group character</returns>
    public static bool IsExplicitGroupCharacter(this Token token) => token.IsExplicitGroupOpener() || token.IsExplicitGroupTerminator();
    public static bool IsValidGroupContent(this Token left, Token right)
    {
        switch (left.Type)
        {
            case TokenType.LeftParenthesis when right.Type is TokenType.RightParenthesis or TokenType.LeftParenthesis or TokenType.LeftBracket:
            case TokenType.RightParenthesis when right.Type is TokenType.LeftParenthesis:
            case TokenType.LeftBracket when right.Type is TokenType.RightBracket:
            case TokenType.RightBracket when right.Type is TokenType.LeftBracket:
                return true;
            default:
                return false;
        }
    }
    
    /// <summary>
    /// Checks if the given <see cref="Token"/> is terminator for a group
    /// </summary>
    /// <param name="token">The <see cref="Token"/> to be checked</param>
    /// <returns>True if the token is a group terminator</returns>
    public static bool IsGroupTerminator(this Token token)
    {
        return token.Type is TokenType.RightParenthesis or TokenType.EndOfFile or TokenType.Comma;
    }
}