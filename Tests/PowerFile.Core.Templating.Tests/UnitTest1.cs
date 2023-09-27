using Microsoft.Extensions.Options;
using NSubstitute;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PowerFile.Core.Templating.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var options = Substitute.For<IOptionsMonitor<TemplatingOptions>>();
        options.CurrentValue.Returns(new TemplatingOptions()
        {
            TemplatesBasePath = @"C:\Users\Jelle\PowerFile\templates",
            IndexPath = @"C:\Users\Jelle\PowerFile\templates/config/templates.index.json",
        });

        var store = new TemplateFileSystemStore(options, new YamlTemplateReader(CreateDeserializer()));
        var templateManager = new TemplateManager(store);
        var template = templateManager.FindTemplates(new[] { "IRandom.cs" });

        int i = 0;
    }

    [Fact]
    public void another()
    {
        var kek = Path.GetFileName("/Features/GaWeg/my_environment.dev.json");
        var kek2 = Path.GetDirectoryName("/Features/GaWeg/");
        int i = 0;
    }

    private IDeserializer CreateDeserializer()
    {
        // TODO: Inject this
        return new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
    }
}