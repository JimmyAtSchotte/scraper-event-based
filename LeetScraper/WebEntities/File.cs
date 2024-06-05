namespace LeetScraper.WebEntities;

public class File : IWebEntity
{
    public File(Uri uri)
    {
        Uri = uri;
    }
    
    public Uri Uri { get; }
    public IEnumerable<Uri> ListLinkedResources() => [];
}