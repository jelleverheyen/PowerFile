using System.Text.RegularExpressions;
using PowerFile.Core.Templating.Abstractions;
using YamlDotNet.Serialization;

namespace PowerFile.Core.Templating;

public class YamlTemplateReader(IDeserializer yamlDeserializer) : ITemplateReader
{
    /// <summary>
    /// Matches Front Matter in the file
    /// </summary>
    private readonly Regex _frontMatterRegex =
        new(@"^---([\S\s]+)---", RegexOptions.NonBacktracking | RegexOptions.Compiled);


    public TemplateParserResult? FromFile(string path)
    {
        var content = File.ReadAllText(path);
        var isSuccess = TryParse(content, out var result);

        return isSuccess 
            ? new TemplateParserResult(result.Content, result.Metadata) 
            : null;
    }

    public bool TryParse(string template, out TemplateParserResult result)
    {
        try
        {
            var match = _frontMatterRegex.Match(template);

            var hasFrontMatter = match.Success;
            if (!hasFrontMatter)
            {
                result = null!;
                return false;
            }

            var group = match.Groups[1];
            var frontMatter = yamlDeserializer.Deserialize<TemplateMetadataDto>(group.Value);
            var metadata = new TemplateMetadata(frontMatter);
            var content = template.Remove(match.Index, match.Length);

            result = new TemplateParserResult(content, metadata);
            
            return true;
        }
        catch (Exception)
        {
            result = null!;
            return false;
        }
        
    }
}

public class TemplateParserResult(string content, TemplateMetadata metadata)
{
    public string Content { get; } = content;
    public TemplateMetadata Metadata { get; } = metadata;
}