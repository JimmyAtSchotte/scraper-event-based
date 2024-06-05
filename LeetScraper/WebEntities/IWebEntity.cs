namespace LeetScraper.WebEntities;

public interface IWebEntity
{
    Uri Uri { get; }
    IEnumerable<Uri> ListLinkedResources();
}