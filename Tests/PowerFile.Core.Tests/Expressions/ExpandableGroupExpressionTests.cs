using NSubstitute;
using PowerFile.Core.Expressions;

namespace PowerFile.Core.Tests.Expressions;

public class ExpandableGroupExpressionTests
{
    [Fact]
    public void Accept_Given_Visitor_Should_Call_VisitExpandableGroupExpression()
    {
        var visitor = Substitute.For<IPowerFileVisitor<object>>();
        var expr = new ExpandableGroupExpression();

        expr.Accept(visitor);

        visitor.Received(1).VisitExpandableGroupExpression(expr);
    }

    [Fact]
    public void Constructor_Given_ListOfExpressions_Should_Correctly_Instantiate()
    {
        var expr = new ExpandableGroupExpression(new List<PowerFileExpression>()
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