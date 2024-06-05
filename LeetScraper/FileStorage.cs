using LeetScraper.WebEntities;
using File = System.IO.File;

namespace LeetScraper;

public class FileStorage
{
    private readonly string _basePath;

    public FileStorage(string basePath)
    {
        _basePath = basePath;
    }
    
    public async Task Store(IWebEntity webEntity)
    {
        var filePath = Path.Combine(_basePath, webEntity.Uri.AbsolutePath.Substring(1));
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        await File.WriteAllBytesAsync(filePath, webEntity.Bytes);
    }
}