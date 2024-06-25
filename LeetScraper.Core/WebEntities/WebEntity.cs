using HtmlAgilityPack;

namespace LeetScraper.Core.WebEntities;

public abstract class WebEntity : IWebEntity
{
    private byte[] _bytes;
    public Uri Uri { get; }
    public byte[] Bytes => _bytes;
    
    protected WebEntity(byte[] bytes, Uri uri)
    {
        _bytes = bytes;
        Uri = uri;
    }
    public abstract IEnumerable<Uri> ListLinkedResources();
    
    private void ReleaseUnmanagedResources()
    {
        _bytes = null;
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~WebEntity()
    {
        ReleaseUnmanagedResources();
    }
}