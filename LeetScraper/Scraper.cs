using LeetScraper;

namespace Tests;

public class Scraper
{
    private readonly HttpClient _httpClient;

    public Scraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public EventHandler<HtmlPage> OnSuccess { get; set; }

    public async Task Scrape(string path, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(path, cancellationToken);
        var htmlPage = new HtmlPage(await response.Content.ReadAsStringAsync(cancellationToken));
        OnSuccess?.Invoke(this, htmlPage);
    }
}