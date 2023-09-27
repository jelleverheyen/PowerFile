using NSubstitute;
using PowerFile.Core.Expressions;

namespace PowerFile.Core.Tests.Expressions;

public class TextGroupExpressionTests
{
    [Fact]
    public void Accept_Given_Visitor_Should_Call_VisitTextGroupExpression()
    {
        var visitor = Substitute.For<IPowerFileVisitor<object>>();
        var expr = new TextGroupExpression();

        expr.Accept(visitor);

        visitor.Received(1).VisitTextGroupExpression(expr);
    }

    [Fact]
    public void Constructor_Given_ListOfExpressions_Should_Correctly_Instantiate()
    {
        var expr = new TextGroupExpression(new List<PowerFileExpression>()
        {
            new TextExpression("Test1"),
            new TextExpression("Test2")
        });

        var children = expr.GetChildren().ToList();
        
        Assert.Equal(2, children.Count);
        Assert.True(children.First() is TextExpression { Text: "Test1" });
        Assert.True(children.Last() is TextExpression { Text: "Test2" });
    }
}