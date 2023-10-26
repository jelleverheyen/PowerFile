using Microsoft.Extensions.Options;
using NSubstitute;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PowerFile.Core.Templating.Tests;

public class TemplateManagerTests
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

    private IDeserializer CreateDeserializer()
    {
        return new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
    }
}