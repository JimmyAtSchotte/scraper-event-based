using ArrangeDependencies.Autofac;
using ArrangeDependencies.Autofac.HttpClient;
using FluentAssertions;
using LeetScraper;

namespace Tests;

[TestFixture]
public class Crawling
{
    [Test]
    public async Task InvokeStatusChanged()
    {
        var baseAddress = new Uri("https://localhost");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, HttpClientConfig.Create(baseAddress, "<html></html>"));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var crawler = new Crawler(scraper, CancellationToken.None);
        var crawlerStatus = default(CrawlerStatus);
        crawler.StatusChanged += (sender, status) => crawlerStatus = status;
        await crawler.BeginCrawling();
        crawlerStatus.Completed.Should().Be(1);
    }
    
    [Test]
    public async Task CrawlLinks()
    {
        var baseAddress = new Uri("https://localhost");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, 
                HttpClientConfig.Create(baseAddress, "<html><body><a href=\"page2.html\"></a></body></html>"),
                HttpClientConfig.Create(new Uri(baseAddress, "page2.html"), "<html></html>"));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var crawler = new Crawler(scraper, CancellationToken.None);
        var crawlerStatus = default(CrawlerStatus);
        crawler.StatusChanged += (sender, status) => crawlerStatus = status;
        await crawler.BeginCrawling();
        crawlerStatus.Completed.Should().Be(2);
    }
    
    [Test]
    public async Task Invoke()
    {
        var baseAddress = new Uri("https://localhost");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, 
            HttpClientConfig.Create(baseAddress, "<html><body></body></html>"));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var crawler = new Crawler(scraper, CancellationToken.None);
        var htmlPage = default(HtmlPage);
        crawler.PageScraped += (sender, page) => htmlPage = page;
        await crawler.BeginCrawling();
        htmlPage.Should().NotBeNull();
    }
    
}