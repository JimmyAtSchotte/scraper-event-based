using System.IO.Abstractions;
using LeetScraper.Core.WebEntities;

namespace LeetScraper.Core;

public class LocalFileStorage
{
    private readonly string _basePath;
    private readonly IFileSystem _fileSystem;

    public LocalFileStorage(IFileSystem fileSystem, string basePath)
    {
        _fileSystem = fileSystem;
        _basePath = basePath;
    }

    public async Task Store(IWebEntity webEntity)
    {
        var filePath = _fileSystem.Path.Combine(_basePath, webEntity.Uri.AbsolutePath.Substring(1));
        _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(filePath));

        await using var fileStream = _fileSystem.File.Create(filePath);
        var stream = webEntity.CreateStream();
        await stream.CopyToAsync(fileStream);
        stream.Close();
        fileStream.Close();
    }

    public IEnumerable<string> CachedStore()
    {
        if (!_fileSystem.Directory.Exists(_basePath))
            return [];
        
        return _fileSystem.Directory.GetFiles(_basePath, "*", SearchOption.AllDirectories)
            .Select(file => file.Substring(_basePath.Length + 1));
        
    }
}