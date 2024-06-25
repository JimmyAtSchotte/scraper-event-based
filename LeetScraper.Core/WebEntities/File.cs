namespace LeetScraper.Core.WebEntities;

public class File : WebEntity
{
    public File(Stream stream, Uri uri) : base(stream, uri)
    {
        
    }
    public override IEnumerable<Uri> ListLinkedResources() => [];
}