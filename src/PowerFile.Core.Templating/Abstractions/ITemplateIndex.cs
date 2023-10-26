namespace PowerFile.Core.Templating.Abstractions;

public interface ITemplateIndex
{
    Template? FindTemplate(string filename, string[]? tags = null);
    void AddTemplate(Template template);
    List<Template> Templates { get; }
}