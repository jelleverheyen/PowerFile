using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using PowerFile.CommandLine.Verbs;
using PowerFile.CommandLine.Verbs.Create;
using PowerFile.CommandLine.Verbs.Preview;
using PowerFile.CommandLine.Verbs.Reload;

namespace PowerFile.CommandLine;

public interface IPowerFileCommandLineApplication
{
    Task<int> RunAsync(IEnumerable<string> args);
}

public class DefaultPowerFileCommandLineApplication(IServiceProvider serviceProvider, Parser parser) : IPowerFileCommandLineApplication
{
    public async Task<int> RunAsync(IEnumerable<string> args)
    {
        var result = parser.ParseArguments(args, new[] { typeof(PreviewVerbOptions), typeof(CreateVerbOptions), typeof(ReloadVerbOptions) });
        await result.WithParsedAsync(Handle);

        return 1;
    }

    private async Task<int> Handle(object o)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        switch (o)
        {
            case PreviewVerbOptions preview:
                return await services.GetRequiredService<IVerbHandler<PreviewVerbOptions>>().HandleAsync(preview);
            case CreateVerbOptions create:
                return await services.GetRequiredService<IVerbHandler<CreateVerbOptions>>().HandleAsync(create);
            case ReloadVerbOptions create:
                return await services.GetRequiredService<IVerbHandler<ReloadVerbOptions>>().HandleAsync(create);
            default: throw new ArgumentOutOfRangeException($"Failed to find verb handler '{o.GetType().FullName}'");
        }
    }
}