using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PowerFile.Core.Templating.Abstractions;
using PowerFile.Core.Templating.Serialization;

namespace PowerFile.Core.Templating;

public class TemplateFileSystemStore
    (IOptionsMonitor<TemplatingOptions> options, ITemplateReader reader) : ITemplateStore
{
    private TemplateIndex? _index;

    public bool HasIndex
    {
        get
        {
            var path = GetIndexPath();

            return File.Exists(path);
        }
    }

    public bool IsIndexLoaded => _index is not null;

    public ITemplateIndex RebuildIndex()
    {
        var templates = (from file in EnumerateTemplateFiles(options.CurrentValue.TemplatesBasePath!)
            select new Template(file)
            into template
            let isSuccess = template.Load(reader)
            where isSuccess
            select template).ToList();

        _index = new TemplateIndex(templates);
        
        StoreIndex();

        return _index;
    }

    public ITemplate? Match(string fileName)
    {
        var index = GetIndex();
        var found = index.FindTemplate(fileName);

        if (found is Template { IsLoaded: false } t)
            t.Load(reader);
        
        return found;
    }

    public bool StoreIndex()
    {
        var path = options.CurrentValue.IndexPath;
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        
        var result = JsonSerializer.Serialize(_index, TemplateIndexJsonSerializerContext.Default.TemplateIndex);
        
        File.WriteAllText(path!, result, Encoding.UTF8);

        return true;
    }

    private ITemplateIndex GetIndex()
    {
        if (_index is not null)
            return _index;

        var result = HasIndex
            ? LoadPersistedIndex()
            : RebuildIndex();

        if (result is null)
            throw new InvalidOperationException("Failed to load index");

        return result;
    }

    private ITemplateIndex? LoadPersistedIndex()
    {
        var indexPath = $"{options.CurrentValue.IndexPath}";
        if (!File.Exists(indexPath))
            throw new InvalidOperationException(
                $"Could not find any persisted index at path \"{indexPath}\", CreateIndex should be called first");

        var file = File.ReadAllText(indexPath);
        _index = JsonSerializer.Deserialize<TemplateIndex>(file, TemplateIndexJsonSerializerContext.Default.TemplateIndex);

        return _index;
    }

    private string GetIndexPath() => $"{options.CurrentValue.IndexPath}";

    /// <summary>
    /// Safely finds all files in the given directory, skips over directories without permissions
    /// </summary>
    /// <param name="basePath">Directory path to start searching from</param>
    /// <returns>List of full paths for all the files that were found</returns>
    private static IEnumerable<string> EnumerateTemplateFiles(string basePath)
    {
        var queue = new Queue<string>();
        queue.Enqueue(basePath);

        while (queue.Any())
        {
            var path = queue.Dequeue();
            try
            {
                foreach (var directory in Directory.GetDirectories(path))
                    queue.Enqueue(directory);
            }
            catch (Exception ex)
            {
                // TODO: Error handling
            }

            var files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(path));
            }
            catch (Exception ex)
            {
                // TODO: Error handling
            }

            if (!files.Any()) continue;

            foreach (var file in files)
                yield return file;
        }
    }
}