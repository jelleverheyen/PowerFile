namespace PowerFile.Core.Templating;

public record TemplatingOptions
{
    public string? TemplatesBasePath { get; set; }
    public string? IndexPath { get; set; }
    
}