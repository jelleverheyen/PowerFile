using NSubstitute;
using PowerFile.Core.Expressions;

namespace PowerFile.Core.Tests.Expressions;

public class TextExpressionTests
{
    [Fact]
    public void Constructor_Given_ValidString_Should_Correctly_Instantiate()
    {
        var expr = new TextExpression("Test");
        Assert.Equal("Test", expr.Text);
    }

    [Fact]
    public void Accept_Given_Visitor_Should_Call_VisitTextExpression()
    {
        var visitor = Substitute.For<IPowerFileVisitor<object>>();
        var expr = new TextExpression("Test");

        expr.Accept(visitor);

        visitor.Received(1).VisitTextExpression(expr);
    }
}