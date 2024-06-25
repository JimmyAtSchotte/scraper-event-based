namespace LeetScraper.Core.WebEntities;

public abstract class WebEntity : IWebEntity
{
    private byte[] _bytes;


    protected WebEntity(byte[] bytes, Uri uri)
    {
        _bytes = bytes;
        Uri = uri;
    }

    public Uri Uri { get; }

    public abstract IEnumerable<Uri> ListLinkedResources();

    public Stream CreateStream()
    {
        return new MemoryStream(_bytes);
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    private void ReleaseUnmanagedResources()
    {
        _bytes = [];
    }

    ~WebEntity()
    {
        ReleaseUnmanagedResources();
    }
}