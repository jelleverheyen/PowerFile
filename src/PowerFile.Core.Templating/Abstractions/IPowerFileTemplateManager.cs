namespace PowerFile.Core.Templating.Abstractions;

public interface IPowerFileTemplateManager
{
    /// <summary>
    /// Finds templates matched with th
    /// </summary>
    /// <param name="fileNames"></param>
    /// <param name="tags">List of tags to limit the templates to</param>
    /// <returns>A dictionary where the key is the fileName, and a list of templates that were found</returns>
    IDictionary<string, ITemplate?> FindTemplates(IEnumerable<string> fileNames, string[]? tags = null);

    /// <summary>
    /// Reloads the template index
    /// </summary>
    /// <returns>True if successful</returns>
    public bool Reload();
}