using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using PowerFile.CommandLine.Verbs;
using PowerFile.Core;
using PowerFile.Core.Templating;
using PowerFile.Core.Templating.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PowerFile.CommandLine;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IPowerFileCommandLineApplication, DefaultPowerFileCommandLineApplication>();
        services.AddTransient<IVerbHandlerFactory, DefaultVerbHandlerFactory>();
        services.AddLogging();
        
        services.AddPowerFile();
        services.AddCommandLineParser();
        
        return services;
    }

    private static IServiceCollection AddPowerFile(this IServiceCollection services)
    {
        services.AddTransient<IPowerFile, DefaultPowerFile>()
            .AddPowerFileTemplating();

        return services;
    }

    private static IServiceCollection AddPowerFileTemplating(this IServiceCollection services)
    {
        services.AddOptions<TemplatingOptions>().Configure((opts) =>
        {
            opts.TemplatesBasePath = @"C:\Users\Jelle\PowerFile\templates";
            opts.IndexPath = @"C:\Users\Jelle\PowerFile\templates/config/templates.index.json";
        });
        
        services.AddSingleton<IPowerFileTemplateManager, TemplateManager>();
        services.AddSingleton<ITemplateStore, TemplateFileSystemStore>();
        services.AddSingleton<ITemplateReader, YamlTemplateReader>();
        services.AddSingleton<IDeserializer>(_ => new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build());

        return services;
    }
    
    private static IServiceCollection AddCommandLineParser(this IServiceCollection services)
    {
        return services.AddSingleton<Parser>(_ =>
        {
            return new Parser(opts =>
            {
                opts.AutoHelp = true;
                opts.CaseSensitive = false;
                opts.GetoptMode = true;
            });
        });
    }
}