using LeetScraper.Core.WebEntities;
using File = LeetScraper.Core.WebEntities.File;

namespace LeetScraper.Core;

public class Scraper
{
    private readonly HttpClient _httpClient;

    public Scraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Func<IWebEntity, Task>? OnSuccess { get; set; }
    public Func<IWebEntity, Task>? OnFailure { get; set; }

    public async Task Scrape(string path, CancellationToken cancellationToken)
    {
        var absoluteUri = new Uri(_httpClient.BaseAddress, path);

        using var response = await _httpClient.GetAsync(path, cancellationToken);
        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            if (OnFailure != null)
                await OnFailure.Invoke(new FailedRequest(bytes, absoluteUri));

            return;
        }

        using IWebEntity entity = response.Content.Headers.ContentType?.MediaType switch
        {
            "text/html" => new HtmlPage(bytes, absoluteUri),
            "text/css" => new StyleSheet(bytes, absoluteUri),
            _ => new File(bytes, absoluteUri)
        };

        await OnSuccess?.Invoke(entity)!;
    }
}