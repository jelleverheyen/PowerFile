using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.Core.Templating;

public class TemplateManager(ITemplateStore templateStore) : IPowerFileTemplateManager
{
    public IDictionary<string, ITemplate?> FindTemplates(IEnumerable<string> fileNames)
    {
        return fileNames
            .Distinct()
            .ToDictionary(
            fileName => fileName,
            templateStore.Match
        );
    }

    public bool Reload()
    {
        templateStore.RebuildIndex();

        return true;
    }
}