namespace PowerFile.Core.Templating;

public class TemplateSearchResult
{
    private readonly IDictionary<int, TemplateMatch> _matches = new Dictionary<int, TemplateMatch>();

    public TemplateMatch? GetResult()
    {
        if (!_matches.Any())
            return null;
        
        return _matches.MaxBy(m => m.Value.Score).Value;
    }
    
    private TemplateMatch GetOrCreateMatch(int templateIndex)
    {
        var exists = _matches.TryGetValue(templateIndex, out var match);
        if (!exists)
        {
            match = new TemplateMatch(templateIndex);
            _matches[templateIndex] = match;
        }

        return match!;
    }
    
    public void SetMatchingPrefix(string prefix, IEnumerable<int> templates)
    {
        foreach (var templateIndex in templates)
        {
            var match = GetOrCreateMatch(templateIndex);
            
            match.AddPrefixMatch(prefix);
        }
    }
    
    public void SetMatchingSuffix(string suffix, IEnumerable<int> templates)
    {
        foreach (var templateIndex in templates)
        {
            var match = GetOrCreateMatch(templateIndex);
            
            match.AddSuffixMatch(suffix);
        }
    }
    public void SetMatchingKeyword(string keyword, IEnumerable<int> templates)
    {
        foreach (var templateIndex in templates)
        {
            var match = GetOrCreateMatch(templateIndex);
            
            match.AddKeywordMatch(keyword);
        }
    }
    public void SetMatchingTags(string tag, IEnumerable<int> templates)
    {
        foreach (var templateIndex in templates)
        {
            var match = GetOrCreateMatch(templateIndex);
            
            match.AddTagMatch(tag);
        }
    }
}

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