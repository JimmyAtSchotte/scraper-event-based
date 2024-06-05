using System.Collections.Concurrent;
using LeetScraper.WebEntities;

namespace LeetScraper;

public class Crawler
{
    private readonly Scraper _scraper;
    private readonly CancellationToken _cancellationToken;
    private CrawlerStatus _crawlerStatus;
    private ConcurrentDictionary<string, bool> _pathsCrawled;
    private object _lock = new object();
    public EventHandler<CrawlerStatus>? StatusChanged { get; set; }
    public EventHandler<IWebEntity>? Scraped { get; set; }

    public Crawler(Scraper scraper, CancellationToken cancellationToken)
    {
        _scraper = scraper;
        _cancellationToken = cancellationToken;
        _scraper.OnSuccess += OnSuccess;
        _crawlerStatus = new CrawlerStatus()
        {
            Completed = 0,
            Total = 1
        };
        _pathsCrawled = new ConcurrentDictionary<string, bool>();
    }
    private async Task OnSuccess(IWebEntity page)
    {
        _pathsCrawled[page.Path] = true;
        
        var tasks = new List<Task>();

        foreach (var resource in page.ListLinkedResources())
        {
            if(_pathsCrawled.TryAdd(resource, false))
                tasks.Add(ScapePage(resource));
        }
        
        _crawlerStatus = new CrawlerStatus
        {
            Completed = _pathsCrawled.Values.Count(completed => completed),
            Total = _pathsCrawled.Values.Count(),
        };
        
        StatusChanged?.Invoke(this, _crawlerStatus);
        Scraped?.Invoke(this, page);

        await Task.WhenAll(tasks);
    }

    public async Task BeginCrawling()
    {
        if(_pathsCrawled.TryAdd("index.html", false))
            await ScapePage("");
    }
    
    private async Task ScapePage(string path)
    {  
        
        await _scraper.Scrape(path, _cancellationToken);
    }
}