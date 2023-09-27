using PowerFile.Core.Expressions;

namespace PowerFile.Core.Visitors;

/// <summary>
/// Used to walk <see cref="PowerFileExpression"/> trees and print them
/// </summary>
public class PowerFileExpressionPrinter : IPowerFileVisitor<IEnumerable<string>>
{
    /// <summary>
    /// Walk the given <see cref="PowerFileExpression"/> and compute lines for printing
    /// </summary>
    /// <param name="expression">The expression tree that needs to be printed</param>
    /// <returns>Array of lines to be printed.</returns>
    public IEnumerable<string> Print(PowerFileExpression expression)
    {
        return expression.Accept(this);
    }
    
    public IEnumerable<string> VisitExpandableGroupExpression(ExpandableGroupExpression expression)
    {
        var result = new List<string>()
        {
            Indent("EXPANDABLE GROUP:"),
            Indent("EXPANDERS:", 2)
        };
        
        foreach (var child in expression.GetChildren())
        {
            result.AddRange(child.Accept(this).Select(i => Indent(i, 2)));
        }

        return result;
    }

    public IEnumerable<string> VisitTextGroupExpression(TextGroupExpression expression)
    {
        var result = new List<string>()
        {
            Indent("TEXT GROUP:"),
            Indent("CHILDREN:", 2)
        };
        
        foreach (var child in expression.GetChildren())
        {
            result.AddRange(child.Accept(this).Select(i => Indent(i, 2)));
        }

        return result;
    }

    public IEnumerable<string> VisitTextExpression(TextExpression expression)
    {
        return new[] { Indent($"TEXT: \"{expression.Text}\"") };
    }

    public IEnumerable<string> VisitRangeExpression(RangeExpression expression)
    {
        return new[] { Indent($"RANGE: [{expression.Start}, {expression.End}]") };
    }

    private static string Indent(string source, int amount = 1)
    {
        var result = source;
        for (var i = 0; i < amount; i++)
        {
            result = "    " + result;
        }

        return result;
    }
}
