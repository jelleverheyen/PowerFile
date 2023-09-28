using System.Text.Json.Serialization;

namespace PowerFile.Core.Templating.Serialization;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(TemplateIndex))]
internal partial class TemplateIndexJsonSerializerContext : JsonSerializerContext
{
    
}