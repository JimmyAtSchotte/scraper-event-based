using HtmlAgilityPack;

namespace LeetScraper.WebEntities;

public class HtmlPage : IWebEntity
{
    private readonly HtmlDocument _htmlDocument;
    
    public HtmlPage(string html, Uri uri)
    {
        Uri = uri.AbsolutePath.EndsWith("/") ? new Uri(uri, "index.html") : uri;
        _htmlDocument = new HtmlDocument();
        _htmlDocument.LoadHtml(html);
    }

    public Uri Uri { get; }

    public IEnumerable<Uri> ListLinkedResources()
    {
        var resources = new List<string>();
        resources.AddRange(ListLinks());
        resources.AddRange(ListImages());
        resources.AddRange(ListCss());
        resources.AddRange(ListJavascript());
        
        return resources.Select(resource => new Uri(Uri, resource));
    }

    private IEnumerable<string> ListLinks()
    {
        return _htmlDocument.DocumentNode.SelectNodes("//a[@href]")?.Select(linkNode => linkNode.GetAttributeValue("href", "")) ?? [];
    }
    
    private IEnumerable<string> ListImages()
    {
        return _htmlDocument.DocumentNode.SelectNodes("//img[@src]")?.Select(linkNode => linkNode.GetAttributeValue("src", "")) ?? [];
    }
    
    private IEnumerable<string> ListCss()
    {
        return _htmlDocument.DocumentNode.SelectNodes("//link[@href]")?.Select(linkNode => linkNode.GetAttributeValue("href", "")) ?? [];
    }
    
    private IEnumerable<string> ListJavascript()
    {
        return _htmlDocument.DocumentNode.SelectNodes("//script[@src]")?.Select(linkNode => linkNode.GetAttributeValue("src", "")) ?? [];
    }
}