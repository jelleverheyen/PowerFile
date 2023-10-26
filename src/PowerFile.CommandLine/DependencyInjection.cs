using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using PowerFile.CommandLine.Config;
using PowerFile.CommandLine.Verbs;
using PowerFile.CommandLine.Verbs.Create;
using PowerFile.CommandLine.Verbs.Preview;
using PowerFile.CommandLine.Verbs.Reload;
using PowerFile.Core;
using PowerFile.Core.Templating;
using PowerFile.Core.Templating.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PowerFile.CommandLine;

public static class DependencyInjection
{
    public static IServiceCollection AddPowerFileCommandLineApplication(this IServiceCollection services)
    {
        services.AddCrossPlatformPaths("PowerFile");
        services.AddTransient<IPowerFileCommandLineApplication, DefaultPowerFileCommandLineApplication>()
            .AddVerbHandlers();
        
        services.AddPowerFile();
        services.AddCommandLineParser();
        
        return services;
    }
    
    private static IServiceCollection AddCrossPlatformPaths(this IServiceCollection services, string applicationName)
    {
        services.AddSingleton<CrossPlatformDirectories>(x =>
        {
            CrossPlatformDirectories? paths = null;

            if (OperatingSystem.IsWindows())
                paths = new WindowsPowerFileDirectories(applicationName);
            if (OperatingSystem.IsLinux())
                paths = new LinuxPowerFileDirectories(applicationName);
            if (OperatingSystem.IsMacOS())
                paths = new MacPowerFileDirectories(applicationName);

            return paths ?? throw new InvalidOperationException($"Unsupported environment, only Windows, Linux, and MacOS are supported.");
        });

        return services;
    }

    private static IServiceCollection AddVerbHandlers(this IServiceCollection services)
    {
        services.AddScoped<IVerbHandler<ReloadVerbOptions>, ReloadVerbHandler>();
        services.AddScoped<IVerbHandler<PreviewVerbOptions>, PreviewVerbHandler>();
        services.AddScoped<IVerbHandler<CreateVerbOptions>, CreateVerbHandler>();
        
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
            var provider = services.BuildServiceProvider();
            var dirs = provider.GetRequiredService<CrossPlatformDirectories>();
            
            opts.TemplatesBasePath = Path.Combine(dirs.Config, "templates");
            opts.IndexPath = Path.Combine(dirs.Cache, "templates.index");

            if (!Directory.Exists(opts.TemplatesBasePath))
            {
                Directory.CreateDirectory(opts.TemplatesBasePath);
            }

            var indexDirectory = Path.GetDirectoryName(opts.IndexPath);
            if (!Directory.Exists(indexDirectory))
            {
                Directory.CreateDirectory(indexDirectory!);
            }
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
                opts.HelpWriter = Console.Error;
            });
        });
    }
}