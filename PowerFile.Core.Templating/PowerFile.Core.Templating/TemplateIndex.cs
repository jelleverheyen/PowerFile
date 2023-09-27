using System.Text.Json.Serialization;
using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.Core.Templating;

public class TemplateIndex : ITemplateIndex
{
    public TemplateIndex()
    {
        Templates = new List<ITemplate>();
        Tags = new Dictionary<string, List<int>>();
        Suffixes = new Dictionary<string, List<int>>();
        Prefixes = new Dictionary<string, List<int>>();
        Keywords = new Dictionary<string, List<int>>();
    }

    public TemplateIndex(IEnumerable<ITemplate> templates,
        Dictionary<string, List<int>> tags,
        Dictionary<string, List<int>> suffixes,
        Dictionary<string, List<int>> prefixes,
        Dictionary<string, List<int>> keywords) => (Templates, Tags, Suffixes, Prefixes, Keywords) =
        (templates.ToList(), tags, suffixes, prefixes, keywords);


    public TemplateIndex(IEnumerable<ITemplate> templates) : this()
    {
        Templates = templates.ToList();

        var current = 0;
        foreach (var template in Templates)
        {
            IndexTemplate(template, current);
            current++;
        }
    }

    public List<ITemplate> Templates { get; init; }
    public IDictionary<string, List<int>> Tags { get; init; }
    public IDictionary<string, List<int>> Suffixes { get; init; }
    public IDictionary<string, List<int>> Prefixes { get; init; }
    public IDictionary<string, List<int>> Keywords { get; init; }

    public ITemplate? FindTemplate(string fileName)
    {
        var search = new TemplateSearchResult();

        foreach (var prefix in Prefixes.Where(p => fileName.StartsWith(p.Key)))
        {
            search.SetMatchingPrefix(prefix.Key, prefix.Value);
        }

        foreach (var suffix in Suffixes.Where(p => fileName.EndsWith(p.Key)))
        {
            search.SetMatchingSuffix(suffix.Key, suffix.Value);
        }

        foreach (var keyword in Keywords.Where(p => fileName.Contains(p.Key)))
        {
            search.SetMatchingKeyword(keyword.Key, keyword.Value);
        }

        foreach (var tag in Tags.Where(p => fileName.Contains(p.Key)))
        {
            search.SetMatchingKeyword(tag.Key, tag.Value);
        }

        var result = search.GetResult();
        return result is null
            ? null
            : Templates[result.TemplateIndex];
    }

    private void IndexTemplate(ITemplate template, int index)
    {
        var metadata = template.Metadata;
        if (metadata is null)
            return;

        foreach (var tag in metadata.Tags)
        {
            if (!Tags.ContainsKey(tag))
                Tags[tag] = new List<int>();

            Tags[tag].Add(index);
        }

        foreach (var prefix in metadata.Prefixes)
        {
            if (!Prefixes.ContainsKey(prefix))
                Prefixes[prefix] = new List<int>();

            Prefixes[prefix].Add(index);
        }

        foreach (var suffix in metadata.Suffixes)
        {
            if (!Suffixes.ContainsKey(suffix))
                Suffixes[suffix] = new List<int>();

            Suffixes[suffix].Add(index);
        }

        foreach (var keyword in metadata.Keywords)
        {
            if (!Keywords.ContainsKey(keyword))
                Keywords[keyword] = new List<int>();

            Keywords[keyword].Add(index);
        }
    }
}