using System.IO.Abstractions;
using LeetScraper.Core.WebEntities;

namespace LeetScraper.Core;

public class LocalFileStorage
{
    private readonly IFileSystem _fileSystem;
    private readonly string _basePath;

    public LocalFileStorage(IFileSystem fileSystem, string basePath)
    {
        _fileSystem = fileSystem;
        _basePath = basePath;
    }
    
    public async Task Store(IWebEntity webEntity)
    {
        var filePath = _fileSystem.Path.Combine(_basePath, webEntity.Uri.AbsolutePath.Substring(1));
        _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(filePath));
        await _fileSystem.File.WriteAllBytesAsync(filePath, webEntity.Bytes);
    }
}