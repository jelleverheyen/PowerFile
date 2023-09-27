using FluentAssertions;
using PowerFile.Core.Scanning;

namespace PowerFile.Core.Tests;

public class ScannerTests
{
    [Fact]
    public void Given_String_Sets_StringLiteral()
    {
        const string value = "Features";
        var scanner = CreateScanner(value).Scan().ToList();
        (scanner.First() is { Lexeme: value, Literal: value, Type: TokenType.String }).Should().BeTrue();
    }
    
    [Fact]
    public void Given_Range_Adds_BracketTokens()
    {
        var tokens = CreateScanner("Test[0,9]").Scan().ToList();

        (tokens[0] is { Type: TokenType.String, Lexeme: "Test" }).Should().BeTrue();
        (tokens[1] is { Type: TokenType.LeftBracket, Lexeme: "[" }).Should().BeTrue();
        (tokens[2] is { Type: TokenType.String, Lexeme: "0" }).Should().BeTrue();
        (tokens[3] is { Type: TokenType.Comma, Lexeme: "," }).Should().BeTrue();
        (tokens[4] is { Type: TokenType.String, Lexeme: "9" }).Should().BeTrue();
        (tokens[5] is { Type: TokenType.RightBracket, Lexeme: "]" }).Should().BeTrue();
        (tokens[6] is { Type: TokenType.EndOfFile }).Should().BeTrue();
    }
    
    [Fact]
    public void Given_Group_Adds_ParenthesisTokens()
    {
        var tokens = CreateScanner("Test(0,9)").Scan().ToList();

        (tokens[0] is { Type: TokenType.String, Lexeme: "Test" }).Should().BeTrue();
        (tokens[1] is { Type: TokenType.LeftParenthesis, Lexeme: "(" }).Should().BeTrue();
        (tokens[2] is { Type: TokenType.String, Lexeme: "0" }).Should().BeTrue();
        (tokens[3] is { Type: TokenType.Comma, Lexeme: "," }).Should().BeTrue();
        (tokens[4] is { Type: TokenType.String, Lexeme: "9" }).Should().BeTrue();
        (tokens[5] is { Type: TokenType.RightParenthesis, Lexeme: ")" }).Should().BeTrue();
        (tokens[6] is { Type: TokenType.EndOfFile }).Should().BeTrue();
    }

    [Fact]
    public void Given_Comma_Adds_CommaToken()
    {
        var tokens = CreateScanner("Test0,Test1,(Test3)").Scan().ToList();
        
        (tokens[0] is { Type: TokenType.String, Lexeme: "Test0" }).Should().BeTrue();
        (tokens[1] is { Type: TokenType.Comma, Lexeme: "," }).Should().BeTrue();
        
        (tokens[2] is { Type: TokenType.String, Lexeme: "Test1" }).Should().BeTrue();
        (tokens[3] is { Type: TokenType.Comma, Lexeme: "," }).Should().BeTrue();
        
        (tokens[4] is { Type: TokenType.LeftParenthesis, Lexeme: "(" }).Should().BeTrue();
        (tokens[5] is { Type: TokenType.String, Lexeme: "Test3" }).Should().BeTrue();
        (tokens[6] is { Type: TokenType.RightParenthesis, Lexeme: ")" }).Should().BeTrue();
        
        (tokens[7] is { Type: TokenType.EndOfFile }).Should().BeTrue();
    }
    
    [Fact]
    public void Given_Pattern_Should_Add_EndOfFile_Token()
    {
        Assert.True(CreateScanner("Test").Scan().Last().Type is TokenType.EndOfFile);
    }

    [Fact]
    public void Given_PatternIncludingDirectorySeparator_Should_AddStringToken()
    {
        var tokens = CreateScanner("Features/(Chat/,Users/)(Commands/,Queries/").Scan();
        Assert.False(tokens.Any(t => t.Type is TokenType.DirectorySeparator));
    }
    
    [Fact]
    public void Given_Pattern_Should_Correctly_Add_Identifier()
    {
        var scanner = CreateScanner("Features/");
        var tokens = scanner.Scan().ToList();
        
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.String, tokens.First().Type);
    }

    private static Scanner CreateScanner(string pattern)
    {
        return new Scanner(pattern);
    }
}