// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.IO.Abstractions;
using LeetScraper;
using LeetScraper.Core;

var uri =  new Uri("https://books.toscrape.com/");

using var client = new HttpClient();
client.BaseAddress = uri;

var statusDisplay = new StatusDisplay(Console.CursorTop, Console.CursorLeft);
var fileStorage = new LocalFileStorage(new FileSystem(), uri.Host);
var scraper = new Scraper(client);
var crawler = new Crawler(scraper, new CancellationToken());
crawler.StatusChanged += () => statusDisplay.Print(crawler);
crawler.Scraped += async (entity) => await fileStorage.Store(entity);

var stopwatch = new Stopwatch();
stopwatch.Start();
await crawler.BeginCrawling();
stopwatch.Stop();

Console.SetCursorPosition(0, Console.CursorTop + 1);
Console.WriteLine($"Total time {stopwatch.Elapsed}");

