using FluentAssertions;
using PowerFile.Core.Parsing;
using PowerFile.Core.Scanning;
using PowerFile.Core.Visitors;

namespace PowerFile.Core.Tests;

public class InterpreterTests
{
    [Fact]
    public void Given_NestedExpression_Should_Correctly_Expand()
    {
        var result = Preview("Features/(Chat/,Users/(Commands/(Create,Update,Delete)/,Queries/(Get,GetAll)/)");
        result.Should().HaveCount(6);
        
        result.Should().Contain("Features/Chat/");
        result.Should().Contain("Features/Users/Commands/Create/");
        result.Should().Contain("Features/Users/Commands/Update/");
        result.Should().Contain("Features/Users/Commands/Delete/");
        result.Should().Contain("Features/Users/Queries/Get/");
        result.Should().Contain("Features/Users/Queries/GetAll/");
    }
    
    [Fact]
    public void Given_NestedRangeExpression_Should_Correctly_Expand()
    {
        var result = Preview("Files/(Chat/[a,z]).json");
        var expected = Enumerable.Range('a', 26).Select(i => $"Files/Chat/{(char) i}.json");

        result.Should().Equal(expected);
    }

    [Fact]
    public void Given_Empty_Commas_Should_Ignore()
    {
        var result = Preview("Features/(Chat,,Users,,,Orders,,,,,,Files)/");
        
        result.Count.Should().Be(4);
        result.Should().Contain("Features/Chat/");
        result.Should().Contain("Features/Users/");
        result.Should().Contain("Features/Orders/");
        result.Should().Contain("Features/Files/");
    }

    [Fact]
    public void Given_Empty_Groups_Should_Ignore()
    {
        var result = Preview("Features/(((()()()())))");
        result.Should().Equal("Features/");
    }

    
    [Fact]
    public void Given_SimpleExpansion_Should_Generate_Correct_Result()
    {
        var result = Preview("Features/(Chat,Users)");
        result.Count.Should().Be(2);
        result[0].Should().Be("Features/Chat");
        result[1].Should().Be("Features/Users");
    }

    [Fact]
    public void Given_DigitRange_Should_Generate_Correct_Result()
    {
        const string pattern = "Features/[0,9]/";
        const string text = "Features/";
        var expected = Enumerable.Range(0, 10).Select(i => text + i + '/');
        var result = Preview(pattern);

        expected.Intersect(result).Should().HaveCount(10);
    }

    [Fact]
    public void Given_CharRange_Should_Generate_Correct_Result()
    {
        
        const string pattern = "Features/[a,c]/";
        const string text = "Features/";
        
        var expected = Enumerable.Range('a', 'c' - 'a' + 1).Select(i => text + (char) i + '/');
        var result = Preview(pattern);

        expected.Intersect(result).Should().HaveCount(3);
    }

    private static List<string> Preview(string pattern)
    {
        return new PowerFileExpressionInterpreter().Interpret(
            new PowerFileParser(
                new Scanner(pattern).Scan().ToList()
            ).Parse()
        ).ToList();
    }
}