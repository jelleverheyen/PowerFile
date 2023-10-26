using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PowerFile.Core.Templating.Abstractions;

public interface ITemplateStore
{
    /// <summary>
    /// Checks whether or not an index already exists, check <see cref="IsIndexLoaded"/> to see if it's been loaded
    /// </summary>
    bool HasIndex { get; }

    /// <summary>
    /// Checks whether or not the template index has already been loaded
    /// </summary>
    bool IsIndexLoaded { get; }

    /// <summary>
    /// Builds and persists
    /// </summary>
    /// <returns></returns>
    ITemplateIndex RebuildIndex();

    /// <summary>
    /// Attempts to match a template with the given file name
    /// </summary>
    /// <param name="fileName">Name of the file to be created</param>
    /// <param name="tags">List of tags to filter the templates by</param>
    /// <returns>The found template if it exists</returns>
    ITemplate? Match(string fileName, string[]? tags = null);
}