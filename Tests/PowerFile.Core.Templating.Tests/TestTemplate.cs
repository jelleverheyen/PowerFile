using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.Core.Templating.Tests;

public class TestTemplate(string path, string content, TemplateMetadata? metadata) : ITemplate
{
    public string Path { get; } = path;
    public string Content { get; } = content;
    public TemplateMetadata? Metadata { get; } = metadata;
}