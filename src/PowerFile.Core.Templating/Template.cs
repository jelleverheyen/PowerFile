using System.Text.Json.Serialization;
using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.Core.Templating;

[method: JsonConstructor]
public class Template(string path) : ITemplate
{
    public Template(string path, TemplateMetadata metadata) : this(path)
    {
        Metadata = metadata;
    }
    
    public Template(string path, string content, TemplateMetadata metadata) : this(path)
    {
        Content = content;
        Metadata = metadata;
        
        IsLoaded = true;
    }
    

    public string? Content { get; private set; }

    public bool IsLoaded { get; private set; }

    public string Path { get; } = path;
    public TemplateMetadata? Metadata { get; private set; }
    
    public bool Load(ITemplateReader reader)
    {
        var template = reader.FromFile(Path);
        
        var isSuccess = template is not null;
        if (isSuccess)
        {
            Content = template?.Content;
            Metadata = template?.Metadata;
        }

        IsLoaded = isSuccess;
        return isSuccess;
    }
}