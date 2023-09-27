using PowerFile.Core.Expressions;

namespace PowerFile.Core.Tests.Helpers;

/// <summary>
/// Helps finding expressions of type '<see cref="T"/>' in the tree
/// </summary>
/// <typeparam name="T">Type of the expression to look for</typeparam>
public class ExpressionFinderVisitor<T> : IPowerFileVisitor<IEnumerable<T>> where T : PowerFileExpression
{
    public IEnumerable<T> Find(PowerFileExpression expression) => expression.Accept(this); 
    public IEnumerable<T> VisitExpandableGroupExpression(ExpandableGroupExpression expandableGroupExpressionTests)
    {
        foreach (var expr in expandableGroupExpressionTests.GetChildren())
        {
            if (expr is T expression)
                yield return expression;
            else
                foreach (var e in expr.Accept(this))
                    yield return e;
        }
    }

    public IEnumerable<T> VisitTextGroupExpression(TextGroupExpression textGroupExpression)
    {
        foreach (var expr in textGroupExpression.GetChildren())
        {
            if (expr is T expression)
                yield return expression;
            else
                foreach (var e in expr.Accept(this))
                    yield return e;
        }
    }

    public IEnumerable<T> VisitTextExpression(TextExpression expression)
    {
        return Enumerable.Empty<T>();
    }

    public IEnumerable<T> VisitRangeExpression(RangeExpression expression)
    {
        if (expression is T e)
            return new List<T> { e };
            
        return Enumerable.Empty<T>();
    }
}