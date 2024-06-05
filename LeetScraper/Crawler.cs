using System.Collections;
using System.Collections.Concurrent;
using LeetScraper.WebEntities;

namespace LeetScraper;

public class Crawler
{
    private readonly Scraper _scraper;
    private readonly CancellationToken _cancellationToken;
    private ConcurrentDictionary<string, bool> _workload;
    public EventHandler? StatusChanged { get; set; }
    public EventHandler<IWebEntity>? Scraped { get; set; }
    public int Completed => _workload.Values.Count(completed => completed);
    public int Total => _workload.Values.Count;
    public IEnumerable NotScraped => _workload.Where(x => x.Value == false).Select(x => x.Key);

    public Crawler(Scraper scraper, CancellationToken cancellationToken)
    {
        _scraper = scraper;
        _cancellationToken = cancellationToken;
        _scraper.OnSuccess += OnSuccess;
        _workload = new ConcurrentDictionary<string, bool>();
    }
    private async Task OnSuccess(IWebEntity page)
    {
        _workload[page.Uri.AbsoluteUri] = true;
        
        var tasks = new List<Task>();

        foreach (var resource in page.ListLinkedResources())
        {
            if(_workload.TryAdd(resource.AbsoluteUri, false))
                tasks.Add(ScapePage(resource.AbsolutePath));
        }
 
        StatusChanged?.Invoke(this, null!);
        Scraped?.Invoke(this, page);

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
}