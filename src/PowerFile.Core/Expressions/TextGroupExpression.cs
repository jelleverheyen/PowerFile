namespace PowerFile.Core.Expressions;

/// <summary>
/// Represents a list of child expressions whose results should be added together
/// </summary>
public class TextGroupExpression : GroupExpression
{
    public TextGroupExpression() { }
    public TextGroupExpression(IEnumerable<PowerFileExpression> expressions) : base(expressions) { }
    
    public override TResult Accept<TResult>(IPowerFileVisitor<TResult> visitor)
    {
        return visitor.VisitTextGroupExpression(this);
    }
}