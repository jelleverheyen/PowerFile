using PowerFile.Core.Exceptions;

namespace PowerFile.Core.Expressions;

/// <summary>
/// Represents a range between two <see cref="int"/> or two <see cref="char"/>s.
/// </summary>
public class RangeExpression : TextGroupExpression
{
    /// <summary>
    /// Start of the range
    /// </summary>
    public string Start { get; }
    
    /// <summary>
    /// End of the range
    /// </summary>
    public string End { get; }
    
    public RangeExpression(int start, int end)
    {
        Start = start.ToString();
        End = end.ToString();
        
        ComputeDigitRange(start, end);
    }
    
    public RangeExpression(string start, string end)
    {
        Start = start.Trim();
        End = end.Trim();
        
        var isStartDigit = int.TryParse(start, out var startNum);
        var isEndDigit = int.TryParse(end, out var endNum);
        
        if (isStartDigit && isEndDigit)
            ComputeDigitRange(startNum, endNum);
        else
        {
            if (start.Length != 1 || end.Length != 1)
                throw new InvalidRangeException("Both range members must be numbers or single characters",
                    $"[{start},{end}]");
                
            ComputeCharRange(start.Single(), end.Single());
        }
    }

    public override TResult Accept<TResult>(IPowerFileVisitor<TResult> visitor)
    {
        return visitor.VisitRangeExpression(this);
    }

    /// <summary>
    /// Compute the range between <see cref="start"/> and <see cref="end"/>
    /// </summary>
    /// <param name="start">Start of the range</param>
    /// <param name="end">End of the range</param>
    /// <exception cref="InvalidRangeException">Thrown when the given <see cref="start"/> and <see cref="end"/> are invalid for a range</exception>
    private void ComputeDigitRange(int start, int end)
    {
        if (start >= end)
            throw new InvalidRangeException($"Range start must be smaller than end", $"[{start}, {end}]");
        
        Children.AddRange(
            Enumerable.Range(start, end - start + 1)
                .Select(c => new TextExpression(c.ToString()))
        );
    }

    /// <summary>
    /// Compute the range between <see cref="start"/> and <see cref="end"/>
    /// </summary>
    /// <param name="start">Start of the range</param>
    /// <param name="end">End of the range</param>
    /// <exception cref="InvalidRangeException">Thrown when the given <see cref="start"/> and <see cref="end"/> are invalid for a range</exception>
    private void ComputeCharRange(char start, char end)
    {
        if (start >= end)
            throw new InvalidRangeException($"Range start must be smaller than end", $"[{start}, {end}]");
        
        Children.AddRange(
            Enumerable.Range(start, end - start + 1)
                .Select(c => new TextExpression(char.ToString((char)c)))
        );
    }
}