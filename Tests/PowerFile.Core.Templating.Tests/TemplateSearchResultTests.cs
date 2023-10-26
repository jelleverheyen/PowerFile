using PowerFile.Core.Templating.Search;

namespace PowerFile.Core.Templating.Tests;

public class TemplateSearchResultTests
{
    [Fact]
    public void Given_SameKeyword_Should_Successfully_Add()
    {
        var searchResult = new TemplateSearchResult();
        searchResult.SetMatchingKeyword("test", new[] { 0 });
        searchResult.SetMatchingKeyword("test", new[] { 1 });

        searchResult.GetResult();
    }
}