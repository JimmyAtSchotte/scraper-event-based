using HtmlAgilityPack;

namespace LeetScraper;

public class HtmlPage
{
    private readonly HtmlDocument _htmlDocument;
    
    public HtmlPage(string html, string path)
    {
        Path = string.IsNullOrEmpty(path) ? "index.html" : path;
        _htmlDocument = new HtmlDocument();
        _htmlDocument.LoadHtml(html);
    }

    public string Path { get; }

    public IEnumerable<string> ListLinkedResources()
    {
       return _htmlDocument.DocumentNode.SelectNodes("//a[@href]")?.Select(linkNode => linkNode.GetAttributeValue("href", "")) ?? [];
    }
}