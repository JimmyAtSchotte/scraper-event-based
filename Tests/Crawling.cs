using System.Text;
using ArrangeDependencies.Autofac;
using ArrangeDependencies.Autofac.HttpClient;
using FluentAssertions;
using LeetScraper;
using LeetScraper.WebEntities;

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
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, HttpClientConfig.Create(baseAddress,  response => response.Content = new StringContent("<html></html>", Encoding.UTF8, "text/html")));
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
                HttpClientConfig.Create(baseAddress,  response => response.Content = new StringContent("<html><body><a href=\"page2.html\"></a></body></html>", Encoding.UTF8, "text/html")),
                HttpClientConfig.Create(new Uri(baseAddress, "page2.html"),  response => response.Content = new StringContent("<html></html>", Encoding.UTF8, "text/html")));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var crawler = new Crawler(scraper, CancellationToken.None);
        var crawlerStatus = default(CrawlerStatus);
        crawler.StatusChanged += (sender, status) => crawlerStatus = status;
        await crawler.BeginCrawling();
        crawlerStatus.Completed.Should().Be(2);
        crawlerStatus.Total.Should().Be(2);
    }
    
    [Test]
    public async Task CrawlOnlyOnce()
    {
        var baseAddress = new Uri("https://localhost");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, 
            HttpClientConfig.Create(baseAddress, response => response.Content = new StringContent("<html><body><a href=\"page2.html\"></a></body></html>", Encoding.UTF8, "text/html")),
            HttpClientConfig.Create(new Uri(baseAddress, "page2.html"), response => response.Content = new StringContent("<html><body><a href=\"index.html\"></a></body></html>", Encoding.UTF8, "text/html")));
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
    public async Task InvokePageScraped()
    {
        var baseAddress = new Uri("https://localhost");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, 
            HttpClientConfig.Create(baseAddress, response => response.Content = new StringContent("<html><body></body></html>", Encoding.UTF8, "text/html")));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var crawler = new Crawler(scraper, CancellationToken.None);
        var htmlPage = default(IWebEntity);
        crawler.Scraped += (sender, entity) => htmlPage = entity;
        await crawler.BeginCrawling();
        htmlPage.Should().NotBeNull();
        htmlPage.Path.Should().Be("index.html");
    }
    
}