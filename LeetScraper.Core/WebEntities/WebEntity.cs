namespace LeetScraper.Core.WebEntities;

public abstract class WebEntity : IWebEntity
{
    private byte[] _bytes;
    
    public Uri Uri { get; }
   
    
    protected WebEntity(byte[] bytes, Uri uri)
    {
        _bytes = bytes;
        Uri = uri;
    }
    
    public abstract IEnumerable<Uri> ListLinkedResources();
    public Stream CreateStream() => new MemoryStream(_bytes);

    private void ReleaseUnmanagedResources()
    {
        _bytes = [];
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