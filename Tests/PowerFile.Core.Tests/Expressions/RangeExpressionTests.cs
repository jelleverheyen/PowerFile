using NSubstitute;
using PowerFile.Core.Exceptions;
using PowerFile.Core.Expressions;

namespace PowerFile.Core.Tests.Expressions;

public class RangeExpressionTests
{
    [Fact]
    public void Accept_Given_Visitor_Calls_VisitRangeExpression()
    {
        var visitor = Substitute.For<IPowerFileVisitor<object>>();
        var range = new RangeExpression(0, 9);
        range.Accept(visitor);

        visitor.Received(1).VisitRangeExpression(range);
    }

    [Fact]
    public void Constructor_Should_Correctly_Set_StartEndProperties()
    {
        var r = new RangeExpression(0, 9);
        Assert.Equal("0", r.Start);
        Assert.Equal("9", r.End);
    }
    
    [Fact]
    public void Constructor_Given_Numbers_Start_Lt_End_Should_Throw_InvalidRangeException()
    {
        Assert.Throws<InvalidRangeException>(() => new RangeExpression(9, 0));
    }
    
    [Fact]
    public void Constructor_Given_Strings_Start_Lt_End_Should_Throw_InvalidRangeException()
    {
        Assert.Throws<InvalidRangeException>(() => new RangeExpression("z", "a"));
    }
    
    [Fact]
    public void Constructor_Given_Numbers_Should_Correctly_Instantiate()
    {
        var range = new RangeExpression(0, 9);

        var result = range.GetChildren().OfType<TextExpression>().Select(e => e.Text)
            .Intersect(Enumerable.Range(0, 10).Select(i => i.ToString()));
        
        Assert.Equal(result.Count(), 10);
    }
    
    [Fact]
    public void Constructor_Given_DigitChars_Should_Correctly_Instantiate()
    {
        var range = new RangeExpression("0", "9");

        var result = range.GetChildren().OfType<TextExpression>().Select(e => e.Text)
            .Intersect(Enumerable.Range(0, 10).Select(i => i.ToString()));
        
        Assert.Equal(result.Count(), 10);
    }
    
    [Fact]
    public void Constructor_Given_Chars_Should_Correctly_Instantiate()
    {
        var range = new RangeExpression("a", "c");

        var result = range.GetChildren().OfType<TextExpression>().Select(e => e.Text)
            .Intersect(Enumerable.Range('a', 'c' - 'a' + 1).Select(i => ((char)i).ToString()));
        
        Assert.Equal(3, result.Count());
    }
    
    [Fact]
    public void Constructor_Given_LargeString_Should_Throw_InvalidRangeException()
    {
        Assert.Throws<InvalidRangeException>(() => new RangeExpression("abc", "def"));
    }
    
    [Fact]
    public void Constructor_Given_LargeNumbers_Should_Correctly_Instantiate()
    {
        var range = new RangeExpression("0000", "1000");

        var result = range.GetChildren().OfType<TextExpression>().Select(e => e.Text)
            .Except(Enumerable.Range(0, 1001).Select(i => i.ToString()));
        
        Assert.Equal(0, result.Count());
    }
}