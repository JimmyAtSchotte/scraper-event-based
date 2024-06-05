using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using LeetScraper;
using LeetScraper.WebEntities;

namespace Tests;

[TestFixture]
public class StoreFileLocally
{
    [Test]
    public async Task Store()
    {
        var fileSystem = new MockFileSystem();
        var fileStorage = new LocalFileStorage(fileSystem, "base");
        var htmlPage = new HtmlPage("<html></html>"u8.ToArray(), new Uri("http://localhost/index.html"));
        await fileStorage.Store(htmlPage);

        fileSystem.FileExists("base/index.html").Should().BeTrue();
    }
}