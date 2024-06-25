using System.Collections.Concurrent;
using LeetScraper.Core.WebEntities;

namespace LeetScraper.Core;

public class Crawler
{
    private readonly CancellationToken _cancellationToken;
    private readonly Scraper _scraper;
    private readonly ConcurrentDictionary<string, ScrapeStatus> _workload;

    public Crawler(Scraper scraper, CancellationToken cancellationToken)
    {
        _scraper = scraper;
        _cancellationToken = cancellationToken;
        _scraper.OnSuccess += OnSuccess;
        _scraper.OnFailure += OnFailure;
        _workload = new ConcurrentDictionary<string, ScrapeStatus>();
    }

    public Action? StatusChanged { get; set; }
    public Func<IWebEntity, Task>? Scraped { get; set; }

    private Task OnFailure(IWebEntity entity)
    {
        _workload[entity.Uri.AbsoluteUri] = ScrapeStatus.Failure;

        StatusChanged?.Invoke();

        return Task.CompletedTask;
    }

    private async Task OnSuccess(IWebEntity page)
    {
        _workload[page.Uri.AbsoluteUri] = ScrapeStatus.Success;

        StatusChanged?.Invoke();
        Scraped?.Invoke(page);
        
        await CrawlUris(page.ListLinkedResources());
    }

    public async Task CrawlUris(IEnumerable<Uri> resources)
    {
        var tasks = (from resource in resources
            where _workload.TryAdd(resource.AbsoluteUri, ScrapeStatus.Pending)
            select ScapePage(resource.AbsolutePath)).ToList();

        await Task.WhenAll(tasks);
    }

    public async Task BeginCrawling()
    {
        await ScapePage("");
    }

    private async Task ScapePage(string path)
    {
        await _scraper.Scrape(path, _cancellationToken);
    }

    public CrawlerStatus GetStatus()
    {
        var completedCount = 0;
        var failedCount = 0;
        var pendingCount = 0;
        var totalCount = _workload.Count;

        foreach (var status in _workload.Values)
        {
            if (status != ScrapeStatus.Pending)
                completedCount++;

            if (status == ScrapeStatus.Failure)
                failedCount++;

            if (status == ScrapeStatus.Pending)
                pendingCount++;
        }

        return new CrawlerStatus(completedCount, totalCount, failedCount, pendingCount);
    }
}