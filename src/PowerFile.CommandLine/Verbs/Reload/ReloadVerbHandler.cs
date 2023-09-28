using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.CommandLine.Verbs.Reload;

public class ReloadVerbHandler
    (ReloadVerbOptions options, IPowerFileTemplateManager templateManager) : VerbHandler<ReloadVerbOptions>(options)
{
    protected override Task<int> HandleAsync(ReloadVerbOptions options, CancellationToken cancellationToken = default)
    {
        var success = templateManager.Reload();

        if (!success)
        {
            Console.WriteLine("An unknown error occurred");
        }
        
        return Task.FromResult(success
            ? 0
            : 1
        );
    }
}