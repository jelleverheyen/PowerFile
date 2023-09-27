namespace PowerFile.Core.Templating.Abstractions;

public interface ITemplateReader
{
    TemplateParserResult? FromFile(string path);
    bool TryParse(string template, out TemplateParserResult result);
}