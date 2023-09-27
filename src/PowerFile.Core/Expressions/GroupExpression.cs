namespace PowerFile.Core.Expressions;

/// <summary>
/// Base class for PowerFile groups.
/// A group is a list of <see cref="PowerFileExpression"/>s that can be evaluated.
/// </summary>
public abstract class GroupExpression : PowerFileExpression
{
    protected readonly List<PowerFileExpression> Children;

    public GroupExpression(IEnumerable<PowerFileExpression> children)
    {
        Children = children.ToList();
    }

    protected GroupExpression()
    {
        Children = new List<PowerFileExpression>();
    }

    public IEnumerable<PowerFileExpression> GetChildren() => Children;
    public void AddChild(PowerFileExpression expression) => Children.Add(expression);

    public override string ToString()
    {
        return $"Group with {Children.Count} elements";
    }
}