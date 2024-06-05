using System.Collections;
using System.Collections.Concurrent;
using LeetScraper.WebEntities;

namespace LeetScraper;

public class Crawler
{
    private readonly Scraper _scraper;
    private readonly CancellationToken _cancellationToken;
    private ConcurrentDictionary<string, ScrapeStatus> _workload;
    public Action? StatusChanged { get; set; }
    public Func<IWebEntity, Task>? Scraped { get; set; }
    public int Completed => _workload.Values.Count(status => status != ScrapeStatus.Pending);
    public int Total => _workload.Values.Count;
    public int Failed => _workload.Values.Count(status => status == ScrapeStatus.Failure);
    public Crawler(Scraper scraper, CancellationToken cancellationToken)
    {
        _scraper = scraper;
        _cancellationToken = cancellationToken;
        _scraper.OnSuccess += OnSuccess;
        _scraper.OnFailure += OnFailure;
        _workload = new ConcurrentDictionary<string, ScrapeStatus>();
    }

    private Task OnFailure(IWebEntity entity)
    {
        _workload[entity.Uri.AbsoluteUri] = ScrapeStatus.Failure;
        
        StatusChanged?.Invoke();

        return Task.CompletedTask;
    }

    private async Task OnSuccess(IWebEntity page)
    {
        _workload[page.Uri.AbsoluteUri] = ScrapeStatus.Success;
        
        var tasks = new List<Task>();

        foreach (var resource in page.ListLinkedResources())
        {
            if(_workload.TryAdd(resource.AbsoluteUri, ScrapeStatus.Pending))
                tasks.Add(ScapePage(resource.AbsolutePath));
        }
 
        StatusChanged?.Invoke();
        Scraped?.Invoke(page);
    }

    public async Task BeginCrawling()
    {
        await ScapePage("");
    }
    
    private async Task ScapePage(string path)
    {  
        await _scraper.Scrape(path, _cancellationToken);
    }
}