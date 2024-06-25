using HtmlAgilityPack;

namespace LeetScraper.Core.WebEntities;

public abstract class WebEntity : IWebEntity
{
    private readonly Stream _stream;
    
    public Uri Uri { get; }
    
    protected WebEntity(Stream stream, Uri uri)
    {
        _stream = stream;
        Uri = uri;
    }
    public abstract IEnumerable<Uri> ListLinkedResources();
    public Stream GetStream()
    {
        _stream.Position = 0;
        return _stream;
    }

    private void ReleaseUnmanagedResources()
    {
        _stream?.Close();
        _stream?.Dispose();
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