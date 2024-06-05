// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using LeetScraper;

using var client = new HttpClient();
client.BaseAddress = new Uri("https://books.toscrape.com/");

var monitor = new object(); 
var cursorTop = Console.CursorTop;
var cursorLeft = Console.CursorLeft;

var scraper = new Scraper(client);
var crawler = new Crawler(scraper, new CancellationToken());
crawler.StatusChanged += () =>
{
    if (!Monitor.TryEnter(monitor)) 
        return;
    
    try
    {
        Console.SetCursorPosition(cursorLeft, cursorTop);
        Console.Write($"Scraped {crawler.Completed} of {crawler.Total}. Failed: {crawler.Failed}");
    }
    finally
    {
        Monitor.Exit(monitor);
    }
};
crawler.Scraped += async (entity) =>
{
    var filePath = Path.Combine(client.BaseAddress.Host, entity.Uri.AbsolutePath.Substring(1));
    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
    await File.WriteAllBytesAsync(filePath, entity.Bytes);
};

var stopwatch = new Stopwatch();
stopwatch.Start();
await crawler.BeginCrawling();
stopwatch.Stop();

Console.SetCursorPosition(cursorLeft, cursorTop + 1);
Console.WriteLine($"Completed at {stopwatch.Elapsed:g}");



