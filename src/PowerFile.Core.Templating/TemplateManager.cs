using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.Core.Templating;

public class TemplateManager(ITemplateStore templateStore) : IPowerFileTemplateManager
{
    public IDictionary<string, ITemplate?> FindTemplates(IEnumerable<string> fileNames, string[]? tags = null)
    {
        return fileNames
            .Distinct()
            .ToDictionary(
                fileName => fileName,
                f => templateStore.Match(f, tags)
            );
    }

    public bool Reload()
    {
        templateStore.RebuildIndex();

        return true;
    }
}