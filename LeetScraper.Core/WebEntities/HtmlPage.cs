using HtmlAgilityPack;

namespace LeetScraper.Core.WebEntities;

public class HtmlPage : WebEntity
{
    private byte[] _bytes;
    
    private const string DefaultAttribute = "";
    
    public HtmlPage(byte[] bytes, Uri uri) : this(new MemoryStream(bytes), uri)
    {
        
    }
    
    public HtmlPage(Stream stream, Uri uri) : base(stream, uri.AbsolutePath.EndsWith("/") ? new Uri(uri, "index.html") : uri)
    {
        
    }
    
    
    public override IEnumerable<Uri> ListLinkedResources()
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.Load(GetStream());
        
        var resources = new List<string>();
        resources.AddRange(ListLinks(htmlDocument));
        resources.AddRange(ListImages(htmlDocument));
        resources.AddRange(ListCss(htmlDocument));
        resources.AddRange(ListJavascript(htmlDocument));
        
        return resources
            .Where(x => (x.StartsWith("http://") || x.StartsWith("https://")) == false)   
            .Where(x => !string.IsNullOrEmpty(x))
            .Where(x => x != DefaultAttribute)
            .Select(resource => new Uri(Uri, resource))
            .ToList();
    }

    private static IEnumerable<string> ListLinks(HtmlDocument htmlDocument)
    {
        return htmlDocument.DocumentNode.SelectNodes("//a[@href]")?.Select(linkNode => linkNode.GetAttributeValue("href", DefaultAttribute)) ?? [];
    }
    
    private static IEnumerable<string> ListImages(HtmlDocument htmlDocument)
    {
        return htmlDocument.DocumentNode.SelectNodes("//img[@src]")?.Select(linkNode => linkNode.GetAttributeValue("src", DefaultAttribute)) ?? [];
    }
    
    private static IEnumerable<string> ListCss(HtmlDocument htmlDocument)
    {
        return htmlDocument.DocumentNode.SelectNodes("//link[@href]")?.Select(linkNode => linkNode.GetAttributeValue("href",DefaultAttribute)) ?? [];
    }
    
    private static IEnumerable<string> ListJavascript(HtmlDocument htmlDocument)
    {
        return htmlDocument.DocumentNode.SelectNodes("//script[@src]")?.Select(linkNode => linkNode.GetAttributeValue("src", DefaultAttribute)) ?? [];
    }
}