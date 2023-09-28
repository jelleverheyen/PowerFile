using CommandLine;
using Microsoft.Extensions.Logging;
using PowerFile.CommandLine.Verbs;
using PowerFile.CommandLine.Verbs.Create;
using PowerFile.CommandLine.Verbs.Preview;
using PowerFile.CommandLine.Verbs.Reload;

namespace PowerFile.CommandLine;

public interface IPowerFileCommandLineApplication
{
    Task<int> RunAsync(IEnumerable<string> args);
}

public class DefaultPowerFileCommandLineApplication(ILogger<DefaultPowerFileCommandLineApplication> logger, Parser parser,
        IVerbHandlerFactory verbHandlerFactory) : IPowerFileCommandLineApplication
{
    public async Task<int> RunAsync(IEnumerable<string> args)
    {
        var handler = parser.ParseArguments<PreviewVerbOptions, CreateVerbOptions, ReloadVerbOptions>(args)
            .MapResult(
                (PreviewVerbOptions opts) => verbHandlerFactory.Create(opts),
                (CreateVerbOptions opts) => verbHandlerFactory.Create(opts),
                (ReloadVerbOptions opts) => verbHandlerFactory.Create(opts),
                    errors => null!
                );

        if (handler is not null)
            return await handler.HandleAsync();

        return 1;
    }
}