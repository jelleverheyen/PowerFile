namespace PowerFile.Core.Templating;

public class TemplateMetadata(TemplateMetadataDto metadata)
{
    public string Name { get; } = metadata.Name ?? string.Empty;
    public string Description { get; } = metadata.Description ?? string.Empty;
    public string[] Tags { get; } = (metadata.Tags ?? string.Empty).Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    public string[] Prefixes { get; } = (metadata.Prefix ?? string.Empty).Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    public string[] Suffixes { get; } = (metadata.Suffix ?? string.Empty).Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    public string[] Keywords { get; } = (metadata.Keywords ?? string.Empty).Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
}

public class TemplateMetadataDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public string? Keywords { get; set; }
    public string? Content { get; set; }
}