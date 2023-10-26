using FluentAssertions;

namespace PowerFile.Core.Templating.Tests;

public class TemplateIndexTests
{
    [Fact]
    public void AddTemplate_Given_ValidTemplate_Should_Correctly_Index_Metadata()
    {
        var sut = new TemplateIndex();
        sut.AddTemplate(new Template(".", new TemplateMetadata()
        {
            Prefixes = new[] { "I" },
            Tags = new[] { "csharp", "C#", "interface"},
            Keywords = new[] { "Interface" },
            Suffixes = new [] { ".cs" }
        }));

        sut.Prefixes.Should().HaveCount(1);
        sut.Prefixes.Keys.Should().Contain(new[] { "I" });
        
        sut.Tags.Should().HaveCount(3);
        sut.Tags.Keys.Should().Contain(new[] { "csharp", "C#", "interface" });
        
        sut.Keywords.Should().HaveCount(1);
        sut.Keywords.Any(s => s.Key == "Interface" && s.Value.SingleOrDefault() == 0).Should().BeTrue();
        
        sut.Suffixes.Should().HaveCount(1);
        sut.Suffixes.Any(s => s.Key == ".cs" && s.Value.SingleOrDefault() == 0).Should().BeTrue();
    }

    [Fact]
    public void AddTemplate_Given_Duplicate_Metadata_Field_Should_Add()
    {
        var sut = new TemplateIndex();

        for (int i = 0; i < 2; i++)
        {
            sut.AddTemplate(new Template(".", new TemplateMetadata()
            {
                Prefixes = new[] { "Test" },
                Keywords = new[] { "Test" },
                Suffixes = new[] { "Test" },
                Tags = new[] { "Test" }
            }));
        }

        sut.Prefixes.Should().HaveCount(1);
        sut.Keywords.Should().HaveCount(1);
        sut.Suffixes.Should().HaveCount(1);
        sut.Tags.Should().HaveCount(1);
    }

    [Fact]
    public void FindTemplate_Given_MatchingPrefix_Should_Match()
    {
        var sut = new TemplateIndex(new[]
        {
            new Template("./", new TemplateMetadata(prefixes: new[] { "I" }))
        });

        var result = sut.FindTemplate("ISomeInterface.cs");

        result.Should().NotBeNull();
        result?.Path.Should().Be("./");
    }

    [Fact]
    public void FindTemplate_Given_MatchingKeyword_Should_Match()
    {
        var sut = new TemplateIndex(new[]
        {
            new Template("./", new TemplateMetadata(keywords: new[] { "RequestHandler" }))
        });

        var result = sut.FindTemplate("SomeRequestHandler.cs");

        result.Should().NotBeNull();
        result?.Path.Should().Be("./");
        result?.Metadata?.Keywords.Should().Contain("RequestHandler");
    }

    [Fact]
    public void FindTemplate_Given_MatchingSuffix_Should_Match()
    {
        var sut = new TemplateIndex(new[]
        {
            new Template("./", new TemplateMetadata(suffixes: new[] { ".cs" }))
        });

        var result = sut.FindTemplate("ISomeInterface.cs");

        result.Should().NotBeNull();
        result?.Path.Should().Be("./");
        result?.Metadata?.Suffixes.Should().Contain(".cs");
    }

    [Fact]
    public void FindTemplate_Given_MatchingTags_Should_Match()
    {
        var sut = new TemplateIndex(new[]
        {
            new Template("./", new TemplateMetadata(tags: new[] { "tag" }))
        });

        var result = sut.FindTemplate("ISomeInterface.cs", tags: new[] { "tag" });

        result.Should().NotBeNull();
        result?.Path.Should().Be("./");
        result?.Metadata?.Tags.Should().Contain("tag");
    }
}