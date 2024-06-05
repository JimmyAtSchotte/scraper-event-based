// See https://aka.ms/new-console-template for more information

using LeetScraper;

using var client = new HttpClient();
client.BaseAddress = new Uri("https://books.toscrape.com/");

var scraper = new Scraper(client);
var crawler = new Crawler(scraper, new CancellationToken());
crawler.StatusChanged += (sender, status) => Console.WriteLine($"Page scraped: {status.Completed}. Total: {status.Total}");
crawler.Scraped += (sender, page) => Console.WriteLine($"Page scraped: {page.Path}");

await crawler.BeginCrawling();

