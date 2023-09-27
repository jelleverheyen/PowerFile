namespace PowerFile.Core.Expressions;

/// <summary>
/// Wraps raw text in an expression
/// </summary>
/// <param name="text">The content of the expression</param>
public class TextExpression(string text) : PowerFileExpression
{
    public string Text { get; } = text;

    public override string ToString()
    {
        return Text;
    }

    public override TResult Accept<TResult>(IPowerFileVisitor<TResult> visitor)
    {
        return visitor.VisitTextExpression(this);
    }
}