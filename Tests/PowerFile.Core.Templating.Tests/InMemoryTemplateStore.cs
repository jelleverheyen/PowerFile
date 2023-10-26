using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.Core.Templating.Tests;

public class InMemoryTemplateStore : ITemplateStore
{
    public bool HasIndex => true;
    public bool IsIndexLoaded => true;


    private ITemplateIndex _index;
    public ITemplateIndex RebuildIndex()
    {
        var templates = new List<Template>()
        {
            new("./test.cs", "public interface $TEST$ { }", new TemplateMetadata("C# Interface", prefixes: new[] { "I" }, suffixes: new[] { ".cs" }))
        };

        _index = new TemplateIndex(templates);
        return _index;
    }

    public ITemplate? Match(string fileName, string[]? paths = null)
    {
        return null;
    }
}