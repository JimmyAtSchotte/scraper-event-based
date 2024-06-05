namespace LeetScraper.Core.WebEntities;

public class FailedRequest : IWebEntity
{
    public FailedRequest(byte[] bytes, Uri uri)
    {
        Uri = uri;
        Bytes = bytes;
    }
    public Uri Uri { get; }
    public byte[] Bytes { get; }
    public IEnumerable<Uri> ListLinkedResources() => [];
}