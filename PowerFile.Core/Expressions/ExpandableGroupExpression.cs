namespace PowerFile.Core.Expressions;

/// <summary>
/// Group whose children are expanded on top of each other
/// </summary>
/// <example>[ a, b, (c, d)] -> [ abc, abd ]</example>
public class ExpandableGroupExpression : GroupExpression
{
    public ExpandableGroupExpression() { }
    public ExpandableGroupExpression(IEnumerable<PowerFileExpression> expressions) : base(expressions) { }
    
    public override TResult Accept<TResult>(IPowerFileVisitor<TResult> visitor)
    {
        return visitor.VisitExpandableGroupExpression(this);
    }
}