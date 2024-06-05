using FluentAssertions;
using LeetScraper;

namespace Tests;

[TestFixture]
public class FindLinkedResources
{
    
    [Test]
    public void NoLinkedResources()
    {
        var htmlPage = new HtmlPage("<html><body></body></html>");
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }
    
    [Test]
    public void Null()
    {
        var htmlPage = new HtmlPage("");
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }
    
    [Test]
    public void Link()
    {
        var htmlPage = new HtmlPage("<html><body><a href=\"page.html\">link</a></body></html>");
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
    }
}