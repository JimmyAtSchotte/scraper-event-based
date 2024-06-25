using System.Text;
using System.Text.RegularExpressions;

namespace LeetScraper.Core.WebEntities;

public class StyleSheet : WebEntity
{
    private static Regex _urlRegex = new Regex(@"url\(['""]?(.*?)['""]?\)", RegexOptions.IgnoreCase);
    
    public StyleSheet(byte[] bytes, Uri uri) : base(bytes, uri)
    {
        
    }

    public override IEnumerable<Uri> ListLinkedResources()
    {
        using var stream = CreateStream();
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        var matches = _urlRegex.Matches(content);
        foreach (Match match in matches)
            if (match.Success)
                yield return new Uri(Uri, match.Groups[1].Value);
    }
}