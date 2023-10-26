namespace PowerFile.Core.Templating.Search;

/// <summary>
/// Utility class for finding the most specific template.
/// Calculates scores based on how many properties are matched as well as how much of the path matches.
/// </summary>
/// <param name="templateIndex">Index of the template, used for tracking</param>
public class TemplateMatch(int templateIndex)
{
    public int TemplateIndex { get; } = templateIndex;

    public int Score { get; private set; }
    public bool IsPrefixMatch { get; private set; }
    public bool IsSuffixMatch { get; private set; }
    public bool IsKeywordMatch { get; private set; }
    public bool IsTagMatch { get; private set; }

    public void AddPrefixMatch(string prefix)
    {
        if (!IsPrefixMatch)
        {
            IsPrefixMatch = true;
            AddScore(20);
        }

        AddScore(prefix.Length);
    }

    public void AddSuffixMatch(string suffix)
    {
        if (!IsSuffixMatch)
        {
            IsSuffixMatch = true;
            AddScore(20);
        }

        AddScore(suffix.Length);
    }

    public void AddKeywordMatch(string keyword)
    {
        if (!IsKeywordMatch)
        {
            IsKeywordMatch = true;
            AddScore(20);
        }

        AddScore(keyword.Length);
    }

    public void AddTagMatch(string tag)
    {
        if (!IsTagMatch)
        {
            IsTagMatch = true;
            AddScore(20);
        }

        AddScore(tag.Length);
    }

    private void AddScore(int score)
    {
        Score += score;
    }
}