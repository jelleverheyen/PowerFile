using Microsoft.Extensions.Options;
using PowerFile.CommandLine.Verbs.Create;
using PowerFile.CommandLine.Verbs.Preview;
using PowerFile.Core;
using PowerFile.Core.Templating;
using PowerFile.Core.Templating.Abstractions;

namespace PowerFile.CommandLine.Verbs;

public interface IVerbHandlerFactory
{
    IVerbHandler Create<TOptions>(TOptions options);
}

public class DefaultVerbHandlerFactory(IPowerFile powerFile, IPowerFileTemplateManager templateManager) : IVerbHandlerFactory
{
    public IVerbHandler Create<TOptions>(TOptions options)
    {
        return options switch
        {
            PreviewVerbOptions o => new PreviewVerbHandler(o, powerFile),
            CreateVerbOptions o => new CreateVerbHandler(o, powerFile, templateManager),
            _ => throw new ArgumentOutOfRangeException(nameof(options))
        };
    }
}