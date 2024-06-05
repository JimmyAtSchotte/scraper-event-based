namespace LeetScraper.WebEntities;

public interface IWebEntity
{
    Uri Uri { get; }
    byte[] Bytes { get; }
    IEnumerable<Uri> ListLinkedResources();
}