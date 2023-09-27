using NSubstitute;
using PowerFile.Core.Exceptions;
using PowerFile.Core.Expressions;
using PowerFile.Core.Parsing;
using PowerFile.Core.Scanning;
using PowerFile.Core.Tests.Helpers;

namespace PowerFile.Core.Tests;

public class ParserTests
{
    [Fact]
    public void Given_Valid_Range_Should_Correctly_Parse()
    {
        var parsed = CreateParser("[0,9]").Parse();
        var range = new ExpressionFinderVisitor<RangeExpression>().Find(parsed).Single();

        Assert.Equal(range.Start, "0");
        Assert.Equal(range.End, "9");
    }

    [Fact]
    public void Given_Invalid_GroupClose_Should_Throw_InvalidGroupException()
    {
        var parser = CreateParser("Features/)");

        Assert.Throws<InvalidGroupException>(parser.Parse);
    }

    [Fact]
    public void Given_Unclosed_Range_Should_Throw_InvalidRangeException()
    {
        var parser = CreateParser("Features/[0");

        Assert.Throws<InvalidRangeException>(parser.Parse);
    }

    [Fact]
    public void Given_LeftParenthesis_Inside_Range_Throws_InvalidRangeException()
    {
        var parser = CreateParser("Features/[0(]");

        Assert.Throws<InvalidRangeException>(parser.Parse);
    }

    [Fact]
    public void Given_RightParenthesis_Inside_Range_Throws_InvalidRangeException()
    {
        var parser = CreateParser("Features/[0)]");

        Assert.Throws<InvalidRangeException>(parser.Parse);
    }

    // [Fact]
    // public void Test1()
    // {
    //     var scanner =
    //         new Scanner("Features/[0,9]/Chat/(Contacts/(Commands/,Queries/,,),Messaging/)spec.(dev,prod).json,Tests");
    //     var tokens = scanner.Scan().ToList();
    //     var parser = new Parser(tokens);
    //     var parsed = parser.Parse();
    //     var interpreted = new Interpreter().Interpret(parsed);
    //
    //     int i = 0;
    // }

    private static PowerFileParser CreateParser(string pattern)
    {
        return new PowerFileParser(
            new Scanner(pattern)
                .Scan()
                .ToList()
        );
    }
}