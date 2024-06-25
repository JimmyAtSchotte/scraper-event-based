namespace LeetScraper.Core.WebEntities;

public class FailedRequest : WebEntity
{
    public FailedRequest(Stream stream, Uri uri) : base(stream, uri)
    {
        
    }
    public override IEnumerable<Uri> ListLinkedResources() => [];
}