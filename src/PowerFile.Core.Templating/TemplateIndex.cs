using PowerFile.Core.Templating.Abstractions;
using PowerFile.Core.Templating.Search;

namespace PowerFile.Core.Templating;

public class TemplateIndex : ITemplateIndex
{
    public TemplateIndex()
    {
        Templates = new List<Template>();
        Tags = new Dictionary<string, List<int>>();
        Suffixes = new Dictionary<string, List<int>>();
        Prefixes = new Dictionary<string, List<int>>();
        Keywords = new Dictionary<string, List<int>>();
    }

    public TemplateIndex(IEnumerable<Template> templates,
        IDictionary<string, List<int>> tags,
        IDictionary<string, List<int>> suffixes,
        IDictionary<string, List<int>> prefixes,
        IDictionary<string, List<int>> keywords)
        => (Templates, Tags, Suffixes, Prefixes, Keywords) = (templates.ToList(), tags, suffixes, prefixes, keywords);


    public TemplateIndex(IEnumerable<Template> templates) : this()
    {
        Templates = templates.ToList();
        foreach (var template in Templates)
        {
            AddTemplate(template);
        }
    }

    public List<Template> Templates { get; init; }
    public IDictionary<string, List<int>> Tags { get; init; }
    public IDictionary<string, List<int>> Suffixes { get; init; }
    public IDictionary<string, List<int>> Prefixes { get; init; }
    public IDictionary<string, List<int>> Keywords { get; init; }

    public void AddTemplate(Template template)
    {
        var addedTemplateIndex = Templates.Count;
        
        var metadata = template.Metadata;
        if (metadata is null)
            return;

        IndexTemplateMetadata(nameof(Tags), addedTemplateIndex, metadata.Tags);
        IndexTemplateMetadata(nameof(Prefixes), addedTemplateIndex, metadata.Prefixes);
        IndexTemplateMetadata(nameof(Suffixes), addedTemplateIndex, metadata.Suffixes);
        IndexTemplateMetadata(nameof(Keywords), addedTemplateIndex, metadata.Keywords);
        
        Templates.AddRange(Templates);
    }

    public Template? FindTemplate(string fileName, string[]? tags = null)
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

        foreach (var tag in Tags.Where(p => (tags ?? Array.Empty<string>()).Contains(p.Key)))
        {
            search.SetMatchingTags(tag.Key, tag.Value);
        }

        var result = search.GetResult();
        return result is null
            ? null
            : Templates[result.TemplateIndex];
    }

    private void IndexTemplateMetadata(string property, int index, string[] values)
    {
        if (values.Length == 0)
            return;

        var list = property switch
        {
            nameof(Tags) => Tags,
            nameof(Prefixes) => Prefixes,
            nameof(Suffixes) => Suffixes,
            nameof(Keywords) => Keywords,
            _ => throw new ArgumentOutOfRangeException(nameof(property), property, "No such template index field exists")
        };

        foreach (var value in values)
        {
            if (!list.ContainsKey(value))
                list[value] = new List<int>();

            list[value].Add(index);
        }
    }
}