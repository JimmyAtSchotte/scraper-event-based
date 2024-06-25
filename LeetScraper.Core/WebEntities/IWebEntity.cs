namespace LeetScraper.Core.WebEntities;

public interface IWebEntity : IDisposable
{
    Uri Uri { get; }
    IEnumerable<Uri> ListLinkedResources();
    Stream GetStream();
}