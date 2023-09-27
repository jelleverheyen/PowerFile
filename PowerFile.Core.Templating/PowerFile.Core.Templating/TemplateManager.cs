using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.Core.Templating;

public class TemplateManager : IPowerFileTemplateManager
{
    private readonly ITemplateStore _templateStore;

    public TemplateManager(ITemplateStore templateStore)
    {
        _templateStore = templateStore;
    }
    
    public IDictionary<string, ITemplate> FindTemplates(string[] fileNames)
    {
        var result = new Dictionary<string, ITemplate>();
        foreach (var fileName in fileNames)
        {
            var template = _templateStore.Match(fileName);
            if (template is null)
                continue;
            
            result.Add(fileName, template);
        }

        return result;
    }

    public bool Reload()
    {
        _templateStore.RebuildIndex();

        return true;
    }
}