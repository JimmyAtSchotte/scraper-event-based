using LeetScraper;

namespace Tests;

public class Crawler
{
    private readonly Scraper _scraper;
    private readonly CancellationToken _cancellationToken;
    private CrawlerStatus _crawlerStatus;
    private List<string> _pathsCrawled;
    public EventHandler<CrawlerStatus>? StatusChanged { get; set; }
    public EventHandler<HtmlPage>? PageScraped { get; set; }


    public Crawler(Scraper scraper, CancellationToken cancellationToken)
    {
        _scraper = scraper;
        _cancellationToken = cancellationToken;
        _scraper.OnSuccess += OnSuccess;
        _crawlerStatus = new CrawlerStatus()
        {
            Completed = 0
        };
        _pathsCrawled = new List<string>();
    }
    private async Task OnSuccess(HtmlPage page)
    {
        _crawlerStatus = new CrawlerStatus { Completed = _crawlerStatus.Completed + 1 };
        StatusChanged?.Invoke(this, _crawlerStatus);
        PageScraped?.Invoke(this, page);
        
        

        foreach (var link in page.ListLinkedResources())
        {
            if (_pathsCrawled.Any(x => x == link)) 
                continue;
         
            await ScapePage(link);
        }
    }

    public async Task BeginCrawling()
    {
        await ScapePage("");
    }
    
    private async Task ScapePage(string path)
    {  
        _pathsCrawled.Add(string.IsNullOrEmpty(path) ? "index.html" : path);
        await _scraper.Scrape(path, _cancellationToken);
    }
}