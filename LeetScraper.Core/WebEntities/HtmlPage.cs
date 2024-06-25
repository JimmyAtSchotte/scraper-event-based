using System.Text;
using System.Text.RegularExpressions;

namespace LeetScraper.Core.WebEntities;

public class HtmlPage : WebEntity
{
    private const string DefaultAttribute = "";
    private static readonly Regex JavascriptRegex = new(@"<script\b[^>]*\bsrc=[""']([^""']+)[""'][^>]*>", RegexOptions.IgnoreCase);
    private static readonly Regex LinksRegex = new(@"<a\b[^>]*\bhref=[""']([^""']+)[""'][^>]*>", RegexOptions.IgnoreCase);
    private static readonly Regex ImagesRegex = new(@"<img\b[^>]*\bsrc=[""']([^""']+)[""'][^>]*>", RegexOptions.IgnoreCase);
    private static readonly Regex CssRegex = new(@"<link\b[^>]*\bhref=[""']([^""']+)[""'][^>]*>", RegexOptions.IgnoreCase);

    public HtmlPage(byte[] bytes, Uri uri) : base(bytes, uri.AbsolutePath.EndsWith("/") ? new Uri(uri, "index.html") : uri)
    {
        
    }
    
    public override IEnumerable<Uri> ListLinkedResources()
    {
        using var stream = CreateStream();
        using var reader = new StreamReader(stream);
        var html = reader.ReadToEnd();
        
        var htmlNoComments = Regex.Replace(html, @"<!--.*?-->", string.Empty, RegexOptions.Singleline);
        
        var resources = new List<string>();
        
        resources.AddRange(LinksRegex.Matches(htmlNoComments).Where(x => x.Success).Select(x => x.Groups[1].Value));
        resources.AddRange(ImagesRegex.Matches(htmlNoComments).Where(x => x.Success).Select(x => x.Groups[1].Value));
        resources.AddRange(CssRegex.Matches(htmlNoComments).Where(x => x.Success).Select(x => x.Groups[1].Value));
        resources.AddRange(JavascriptRegex.Matches(htmlNoComments).Where(x => x.Success).Select(x => x.Groups[1].Value));
   
        var uris = resources
            .Where(x => (x.StartsWith("http://") || x.StartsWith("https://") || x.StartsWith("//")) == false)   
            .Where(x => !string.IsNullOrEmpty(x))
            .Where(x => x != DefaultAttribute)
            .Select(resource => new Uri(Uri, resource))
            .ToList();
        
        return uris;
    }
}