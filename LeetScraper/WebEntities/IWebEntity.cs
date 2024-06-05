namespace LeetScraper.WebEntities;

public interface IWebEntity
{
    string Path { get; }
    IEnumerable<string> ListLinkedResources();
}