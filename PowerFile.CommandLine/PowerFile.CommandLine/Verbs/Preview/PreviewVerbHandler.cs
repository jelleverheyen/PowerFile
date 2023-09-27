using PowerFile.Core;
using PowerFile.Core.Visitors;

namespace PowerFile.CommandLine.Verbs.Preview;

public class PreviewVerbHandler(PreviewVerbOptions options, IPowerFile powerFile) : VerbHandler<PreviewVerbOptions>(options)
{
    protected override Task<int> HandleAsync(PreviewVerbOptions options, CancellationToken cancellationToken = default)
    {
        var lines = options.Debug 
            ? new PowerFileExpressionPrinter().Print(powerFile.Parse(options.Pattern)) 
            : powerFile.Preview(options.Pattern);
        
        if (options.Limit is > 0)
            lines = lines.Take(options.Limit.Value);
        
        foreach (var line in lines)
        {
            Console.WriteLine("- " + line);
        }

        return Task.FromResult(0);
    }
}