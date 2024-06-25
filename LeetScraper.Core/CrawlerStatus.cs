namespace LeetScraper.Core;

public record CrawlerStatus(int Completed, int Total, int Failed, int Pending)
{
}