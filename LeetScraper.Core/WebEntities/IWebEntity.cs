namespace LeetScraper.Core.WebEntities;

public interface IWebEntity : IDisposable
{
    Uri Uri { get; }
    byte[] Bytes { get; }
    IEnumerable<Uri> ListLinkedResources();
}