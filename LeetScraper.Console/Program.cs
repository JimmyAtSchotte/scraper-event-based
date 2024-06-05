// See https://aka.ms/new-console-template for more information

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
crawler.StatusChanged += () => statusDisplay.Print(crawler.Completed, crawler.Total, crawler.Failed);
crawler.Scraped += async (entity) => await fileStorage.Store(entity);
await crawler.BeginCrawling();
