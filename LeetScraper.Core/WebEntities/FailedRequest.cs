namespace LeetScraper.Core.WebEntities;

public class FailedRequest : WebEntity
{
    public FailedRequest(byte[] bytes, Uri uri) : base(bytes, uri) { }

    public override IEnumerable<Uri> ListLinkedResources()
    {
        return [];
    }
}