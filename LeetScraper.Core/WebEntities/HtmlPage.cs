using HtmlAgilityPack;

namespace LeetScraper.Core.WebEntities;

public class HtmlPage : IWebEntity
{
    private const string DefaultAttribute = "";
    
    private readonly HtmlDocument _htmlDocument;
    
    public HtmlPage(byte[] bytes, Uri uri)
    {
        Bytes = bytes;
        Uri = uri.AbsolutePath.EndsWith("/") ? new Uri(uri, "index.html") : uri;
        _htmlDocument = new HtmlDocument();
        _htmlDocument.Load(new MemoryStream(bytes));
    }

    public Uri Uri { get; }
    public byte[] Bytes { get; }

    public IEnumerable<Uri> ListLinkedResources()
    {
        var resources = new List<string>();
        resources.AddRange(ListLinks());
        resources.AddRange(ListImages());
        resources.AddRange(ListCss());
        resources.AddRange(ListJavascript());
        
        return resources
            .Where(x => (x.StartsWith("http://") || x.StartsWith("https://")) == false)   
            .Where(x => !string.IsNullOrEmpty(x))
            .Where(x => x != DefaultAttribute)
            .Select(resource => new Uri(Uri, resource));
    }

    private IEnumerable<string> ListLinks()
    {
        return _htmlDocument.DocumentNode.SelectNodes("//a[@href]")?.Select(linkNode => linkNode.GetAttributeValue("href", DefaultAttribute)) ?? [];
    }
    
    private IEnumerable<string> ListImages()
    {
        return _htmlDocument.DocumentNode.SelectNodes("//img[@src]")?.Select(linkNode => linkNode.GetAttributeValue("src", DefaultAttribute)) ?? [];
    }
    
    private IEnumerable<string> ListCss()
    {
        return _htmlDocument.DocumentNode.SelectNodes("//link[@href]")?.Select(linkNode => linkNode.GetAttributeValue("href",DefaultAttribute)) ?? [];
    }
    
    private IEnumerable<string> ListJavascript()
    {
        return _htmlDocument.DocumentNode.SelectNodes("//script[@src]")?.Select(linkNode => linkNode.GetAttributeValue("src", DefaultAttribute)) ?? [];
    }
}