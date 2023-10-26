namespace PowerFile.Core.Templating.Search;

public class TemplateSearchResult
{
    private readonly Dictionary<int, TemplateMatch> _matches = new();

    public TemplateMatch? GetResult()
    {
        return _matches.Count != 0
            ? _matches.MaxBy(m => m.Value.Score).Value 
            : null;
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