using FluentAssertions;
using LeetScraper;

namespace Tests;

[TestFixture]
public class FindLinks
{
    [Test]
    public void Ahref()
    {
        var htmlPage = new HtmlPage("<html><body><a href=\"page.html\">link</a>");
        var links = htmlPage.ListLinks();
        links.Should().HaveCount(1);
    }
}