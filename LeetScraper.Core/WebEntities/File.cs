namespace LeetScraper.Core.WebEntities;

public class File : WebEntity
{
    public File(byte[] bytes, Uri uri) : base(bytes, uri)
    {
        
    }
    public override IEnumerable<Uri> ListLinkedResources() => [];
}