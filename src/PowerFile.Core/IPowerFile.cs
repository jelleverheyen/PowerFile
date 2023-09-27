using PowerFile.Core.Exceptions;
using PowerFile.Core.Expressions;
using PowerFile.Core.Parsing;
using PowerFile.Core.Scanning;

namespace PowerFile.Core;

/// <summary>
/// Entry point for working with core language features of PowerFile
/// </summary>
public interface IPowerFile
{
    /// <summary>
    /// Get the result of the given pattern, without creating resources
    /// </summary>
    /// <param name="pattern">Pattern to be evaluated</param>
    /// <returns>Result of the pattern</returns>
    IEnumerable<string> Preview(string pattern);

    /// <summary>
    /// Parses the given pattern
    /// </summary>
    /// <param name="pattern">Pattern to be parsed</param>
    /// <returns>The resulting expression</returns>
    PowerFileExpression Parse(string pattern);
    
    /// <summary>
    /// Check given pattern for parsing errors
    /// </summary>
    /// <param name="pattern">The pattern to be evaluated</param>
    /// <returns>True if valid pattern, False if parsing errors were found</returns>
    bool Validate(string pattern);
}

public class DefaultPowerFile : IPowerFile, IPowerFileVisitor<IReadOnlyCollection<string>>
{
    public DefaultPowerFile()
    {
        
    }
    
    public IEnumerable<string> Preview(string pattern)
    {
        return GetExpressionTree(pattern).Accept(this);
    }

    public PowerFileExpression Parse(string pattern)
    {
        return GetExpressionTree(pattern);
    }

    public bool Validate(string pattern)
    {
        try
        {
            GetExpressionTree(pattern);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static PowerFileExpression GetExpressionTree(string pattern)
    {
        try
        {
            var tokens = new Scanner(pattern).Scan();
            var parsed = new PowerFileParser(tokens);

            return parsed.Parse();
        }
        catch (ParserException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
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
