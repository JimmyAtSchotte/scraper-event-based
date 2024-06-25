using FluentAssertions;
using LeetScraper.Core.WebEntities;

namespace Tests;

[TestFixture]
public class FindLinkedResources
{
    [Test]
    public void NoLinkedResources()
    {
        var htmlPage = new HtmlPage("<html><body></body></html>"u8.ToArray(), new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }

    [Test]
    public void Null()
    {
        var htmlPage = new HtmlPage(""u8.ToArray(), new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }

    [Test]
    public void Link()
    {
        var htmlPage = new HtmlPage("<html><body><a href=\"page.html\">link</a></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
    }
    
    [Test]
    public void LinkInHtmlComment()
    {
        var htmlPage = new HtmlPage("<html><body><!--<a href=\"page.html\">link</a>--></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }

    [Test]
    public void LinkEmptyHref()
    {
        var htmlPage = new HtmlPage("<html><body><a>link</a></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }
    
    [Test]
    public void RelativeLink()
    {
        var htmlPage = new HtmlPage("<html><body><a href=\"../page.html\">link</a></body></html>"u8.ToArray(),
        new Uri("http://localhost/pages/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.ElementAt(0).AbsolutePath.Should().Be("/page.html");
    }
    
    [Test]
    public void SkipExternalLinks()
    {
        var htmlPage = new HtmlPage("<html><body><a href=\"http://google.com\">link</a><a href=\"//google.com\">link</a></body></html>"u8.ToArray(),
        new Uri("http://localhost/pages/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().BeEmpty();
    }

    [Test]
    public void Images()
    {
        var htmlPage = new HtmlPage("<html><body><img src=\"image.jpg\"></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
    }
    
    [Test]
    public void ImagesEmptySrc()
    {
        var htmlPage = new HtmlPage("<html><body><img src></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }

    [Test]
    public void Css()
    {
        var htmlPage = new HtmlPage("<html><link href=\"css.css\"></link><body></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
    }

    [Test]
    public void CssEmptyHref()
    {
        var htmlPage = new HtmlPage("<html><link href></link><body></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }

    [Test]
    public void Javascript()
    {
        var htmlPage = new HtmlPage("<html><script src=\"js.js\"></link><body></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
    }
    
    
    [Test]
    public void JavascriptEmptySrc()
    {
        var htmlPage = new HtmlPage("<html><script src></link><body></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(0);
    }
    
    
    [Test]
    public void StyleSheetUrl()
    {
        var htmlPage = new StyleSheet("@font-face {\n  font-family: 'Glyphicons Halflings';\n  src: url('../fonts/glyphicons-halflings-regular.eot');\n  src: url('../fonts/glyphicons-halflings-regular.eot%3F') format('embedded-opentype'), url('../fonts/glyphicons-halflings-regular.woff') format('woff'), url('../fonts/glyphicons-halflings-regular.ttf') format('truetype'), url('../fonts/glyphicons-halflings-regular.svg') format('svg');\n}"u8.ToArray(),
        new Uri("http://localhost/css/css.css"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(5);
        resources.ElementAt(0).AbsolutePath.Should().Be("/fonts/glyphicons-halflings-regular.eot");
    }
    
    [Test]
    public void JavascriptDocumentWrite()
    {
        var htmlPage = new HtmlPage("<html><body><script>window.jQuery || document.write('<script src=\"static/oscar/js/jquery/jquery-1.9.1.min.js\"><\\/script>')</script></body></html>"u8.ToArray(),
        new Uri("http://localhost/index.html"));
        var resources = htmlPage.ListLinkedResources();
        resources.Should().HaveCount(1);
        resources.ElementAt(0).AbsolutePath.Should().Be("/static/oscar/js/jquery/jquery-1.9.1.min.js");
    }
    
    
}