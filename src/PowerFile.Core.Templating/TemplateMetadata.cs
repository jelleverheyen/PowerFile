namespace PowerFile.Core.Templating;

public class TemplateMetadata
{
    public TemplateMetadata()
    {
        Tags = Array.Empty<string>();
        Prefixes = Array.Empty<string>();
        Suffixes = Array.Empty<string>();
        Keywords = Array.Empty<string>();
    }

    public TemplateMetadata(TemplateMetadataDto metadata)
    {
        Name = metadata.Name ?? string.Empty;
        Description = metadata.Description ?? string.Empty;
        
        Tags = (metadata.Tags ?? string.Empty).Split(' ',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        Prefixes = (metadata.Prefix ?? string.Empty).Split(' ',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        Suffixes = (metadata.Suffix ?? string.Empty).Split(' ',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        Keywords = (metadata.Keywords ?? string.Empty).Split(' ',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public string? Name { get; }
    public string? Description { get; }

    public string[] Tags { get; init; }

    public string[] Prefixes { get; init; }

    public string[] Suffixes { get; init; }

    public string[] Keywords { get; init; }
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