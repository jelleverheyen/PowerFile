namespace PowerFile.Core.Expressions;

/// <summary>
/// Interface for walking a tree of <see cref="PowerFileExpression"/>s
/// </summary>
/// <typeparam name="TResult">The result of the Visitor</typeparam>
public interface IPowerFileVisitor<out TResult>
{
    TResult VisitExpandableGroupExpression(ExpandableGroupExpression expression);
    TResult VisitTextGroupExpression(TextGroupExpression expression);
    TResult VisitTextExpression(TextExpression expression);
    TResult VisitRangeExpression(RangeExpression expression);
}