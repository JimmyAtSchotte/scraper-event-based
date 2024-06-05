namespace LeetScraper.Core.WebEntities;

public class File : IWebEntity
{
    public File(byte[] bytes, Uri uri)
    {
        Uri = uri;
        Bytes = bytes;
    }
    
    public Uri Uri { get; }
    public byte[] Bytes { get; }
    public IEnumerable<Uri> ListLinkedResources() => [];
}