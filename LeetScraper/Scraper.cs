namespace LeetScraper;

public class Scraper
{
    private readonly HttpClient _httpClient;

    public Scraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Func<HtmlPage, Task>? OnSuccess { get; set; }

    public async Task Scrape(string path, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(path, cancellationToken);
        var htmlPage = new HtmlPage(await response.Content.ReadAsStringAsync(cancellationToken));
        
        if (OnSuccess == null)
            return;

        await OnSuccess.Invoke(htmlPage);
    }
}