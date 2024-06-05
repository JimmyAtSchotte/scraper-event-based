namespace LeetScraper.WebEntities;

public class File : IWebEntity
{
    public File(string path)
    {
        Path = path;
    }
    public string Path { get; }
    
    public IEnumerable<string> ListLinkedResources() => [];
}