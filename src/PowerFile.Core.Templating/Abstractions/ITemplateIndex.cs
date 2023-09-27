namespace PowerFile.Core.Templating.Abstractions;

public interface ITemplateIndex
{
    ITemplate? FindTemplate(string filename);
    List<Template> Templates { get; }
}