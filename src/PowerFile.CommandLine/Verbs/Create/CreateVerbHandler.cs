using PowerFile.Core;
using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.CommandLine.Verbs.Create;

public class CreateVerbHandler(IPowerFile powerFile,
    IPowerFileTemplateManager templateManager) : IVerbHandler<CreateVerbOptions>
{
    
    public Task<int> HandleAsync(CreateVerbOptions options, CancellationToken cancellationToken = default)
    {
        var paths = powerFile.Preview(options.Pattern)
            .Select(path =>
            {
                var directoryName = Path.GetDirectoryName(path) ?? string.Empty;
                var fileName = Path.GetFileName(path) ?? string.Empty;

                return new
                {
                    FullPath = Path.Combine(Environment.CurrentDirectory, directoryName, fileName),
                    RelativePath = Path.Combine(directoryName, fileName),
                    FileName = !string.IsNullOrWhiteSpace(fileName) ? fileName : null,
                    Directory = !string.IsNullOrWhiteSpace(directoryName) ? directoryName : null
                };
            }).ToArray();

        Console.WriteLine("Attempting to create files:");
        foreach (var path in paths.Select(f => f.RelativePath))
        {
            Console.WriteLine($"- {path}");
        }
        
        var fileNames = paths
            .Where(f => f.FileName is not null)
            .Select(f => f.FileName!)
            .ToList();
        
        var templates = templateManager.FindTemplates(fileNames.ToArray(), options.Tags.ToArray());
        
        foreach (var path in paths)
        {
            if (path.Directory is not null)
                Directory.CreateDirectory(path.Directory);

            if (path.FileName is null) 
                continue;
            
            var template = templates[path.FileName];
            if (string.IsNullOrWhiteSpace(template?.Content))
                File.Create(path.FullPath);
            else
                File.WriteAllText(path.FullPath, template.Content);
        }

        return Task.FromResult(0);
    }
}