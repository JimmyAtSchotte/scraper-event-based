namespace LeetScraper;

public record CrawlerStatus
{
    public int Completed { get; init; }
    public int Total { get; init; }
}