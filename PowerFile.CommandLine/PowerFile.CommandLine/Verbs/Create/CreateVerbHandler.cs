using PowerFile.Core;
using PowerFile.Core.Templating;
using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.CommandLine.Verbs.Create;

public class CreateVerbHandler(CreateVerbOptions options, IPowerFile powerFile,
    IPowerFileTemplateManager templateManager) : IVerbHandler
{
    public Task<int> HandleAsync(CancellationToken cancellationToken = default)
    {
        var files = powerFile.Preview(options.Pattern)
            .Select(path =>
            {
                var directoryName = Path.GetDirectoryName(path);
                var fileName = Path.GetFileName(path);

                return new
                {
                    FullPath = $"{Environment.CurrentDirectory}/{directoryName}/{fileName}".Replace("//", "/"),
                    FileName = !string.IsNullOrWhiteSpace(fileName) ? fileName : null,
                    Directory = !string.IsNullOrWhiteSpace(directoryName) ? directoryName : null
                };
            }).ToArray();

        var fileNames = files
            .Where(f => f.FileName is not null)
            .Select(f => f.FileName!);

        var templates = templateManager.FindTemplates(fileNames.ToArray());

        foreach (var file in files)
        {
            if (file.Directory is not null)
                Directory.CreateDirectory(file.Directory);
            
            if (file.FileName is not null)
            {
                var template = templates[file.FileName];
                File.WriteAllText(file.FullPath, template.Content);
            }
        }

        return Task.FromResult(0);
    }
}