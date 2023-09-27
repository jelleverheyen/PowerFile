using PowerFile.Core.Expressions;

namespace PowerFile.Core.Visitors;

public class PowerFileExpressionInterpreter : IPowerFileVisitor<IReadOnlyCollection<string>>
{
    public IEnumerable<string> Interpret(PowerFileExpression expression)
    {
        return expression.Accept(this);
    }

    public IReadOnlyCollection<string> VisitExpandableGroupExpression(ExpandableGroupExpression expression)
    {
        var result = new List<string>();

        return expression.GetChildren()
            .Aggregate(
                result,
                (current, expander) => Combine(current, expander.Accept(this)).ToList()
            );
    }

    public IReadOnlyCollection<string> VisitTextGroupExpression(TextGroupExpression expression)
    {
        var result = new List<string>();
        foreach (var child in expression.GetChildren())
        {
            result.AddRange(child.Accept(this));
        }

        return result;
    }

    public IReadOnlyCollection<string> VisitTextExpression(TextExpression expression)
    {
        return new[] { expression.Text };
    }

    public IReadOnlyCollection<string> VisitRangeExpression(RangeExpression expression)
    {
        return VisitTextGroupExpression(expression);
    }

    private static IEnumerable<string> Combine(IReadOnlyCollection<string> left,
        IReadOnlyCollection<string> right)
    {
        if (!left.Any())
            return right;
        if (!right.Any())
            return left;

        return from r in right from l in left select l + r;
    }
}