using LeetScraper.WebEntities;
using File = LeetScraper.WebEntities.File;

namespace LeetScraper;

public class Scraper
{
    private readonly HttpClient _httpClient;

    public Scraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Func<IWebEntity, Task>? OnSuccess { get; set; }

    public async Task Scrape(string path, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(path, cancellationToken);
        
        if(!response.IsSuccessStatusCode)
            return;
        
        if (OnSuccess == null)
            return;
        
        IWebEntity entity = response.Content.Headers.ContentType?.MediaType switch
        {
            "text/html" => new HtmlPage(await response.Content.ReadAsStringAsync(cancellationToken), path),
            _ => new File(path)
        };
        
        await OnSuccess.Invoke(entity);
    }
}