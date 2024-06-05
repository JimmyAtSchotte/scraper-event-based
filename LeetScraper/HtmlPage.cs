using HtmlAgilityPack;

namespace LeetScraper;

public class HtmlPage
{
    private readonly HtmlDocument _htmlDocument;
    
    public HtmlPage(string html)
    {
        _htmlDocument = new HtmlDocument();
        _htmlDocument.LoadHtml(html);
    }

    public IEnumerable<string> ListLinkedResources()
    {
       return _htmlDocument.DocumentNode.SelectNodes("//a[@href]")?.Select(linkNode => linkNode.GetAttributeValue("href", "")) ?? [];
    }
}