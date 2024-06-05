using FluentAssertions;
using LeetScraper;
using LeetScraper.WebEntities;

namespace Tests;

[TestFixture]
public class FindLinkedResources
{
    [Test]
    public void NoLinkedResources()
    {
        var htmlPage = new HtmlPage("<html><body></body></html>", new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }

    [Test]
    public void Null()
    {
        var htmlPage = new HtmlPage("", new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }

    [Test]
    public void Link()
    {
        var htmlPage = new HtmlPage("<html><body><a href=\"page.html\">link</a></body></html>",
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
    }

    [Test]
    public void RelativeLink()
    {
        var htmlPage = new HtmlPage("<html><body><a href=\"../page.html\">link</a></body></html>",
        new Uri("http://localhost/pages/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.ElementAt(0).AbsolutePath.Should().Be("/page.html");
    }

    [Test]
    public void Images()
    {
        var htmlPage = new HtmlPage("<html><body><img src=\"image.jpg\"></body></html>",
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
    }

    [Test]
    public void Css()
    {
        var htmlPage = new HtmlPage("<html><link href=\"css.css\"></link><body></body></html>",
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
    }

    [Test]
    public void Javascript()
    {
        var htmlPage = new HtmlPage("<html><script src=\"js.js\"></link><body></body></html>",
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
    }
}