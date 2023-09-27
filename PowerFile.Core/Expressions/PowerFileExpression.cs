namespace PowerFile.Core.Expressions;

/// <summary>
/// Base class for PowerFile Expressions
/// </summary>
public abstract class PowerFileExpression
{
    /// <summary>
    /// Accept the <see cref="IPowerFileVisitor{TResult}"/> and call the correct visiting method
    /// </summary>
    /// <param name="visitor">The visitor to be accepted</param>
    /// <typeparam name="TResult">Type of the result</typeparam>
    /// <returns>The result of the expression evaluation</returns>
    public abstract TResult Accept<TResult>(IPowerFileVisitor<TResult> visitor);
}