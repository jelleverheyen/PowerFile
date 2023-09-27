using System.Text.Json.Serialization;

namespace PowerFile.Core.Templating.Abstractions;

[JsonDerivedType(typeof(Template), "template")]
public interface ITemplate
{
    public string Path { get; }
    
    [JsonIgnore]
    public string Content { get; }
    
    [JsonIgnore]
    public TemplateMetadata? Metadata { get; }
}