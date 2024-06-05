using System.Net;
using System.Text;
using ArrangeDependencies.Autofac;
using ArrangeDependencies.Autofac.HttpClient;
using FluentAssertions;
using LeetScraper.Core;
using LeetScraper.Core.WebEntities;

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
        var invokes = 0;
        crawler.StatusChanged += () => invokes++;
        await crawler.BeginCrawling();
        crawler.Completed.Should().Be(1);
        invokes.Should().Be(1);
    }
    
    [Test]
    public async Task CrawlLinks()
    {
        var baseAddress = new Uri("https://localhost");
        var page2Uri = new Uri(baseAddress, "page2.html");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, 
                HttpClientConfig.Create(baseAddress,  response => response.Content = new StringContent("<html><body><a href=\"page2.html\"></a></body></html>", Encoding.UTF8, "text/html")),
                HttpClientConfig.Create(page2Uri,  response => response.Content = new StringContent("<html></html>", Encoding.UTF8, "text/html")));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var crawler = new Crawler(scraper, CancellationToken.None);
        var invokes = 0;
        var scraped = new List<IWebEntity>();
        crawler.StatusChanged += () => invokes++;
        crawler.Scraped += (entity) =>
        {
            scraped.Add(entity);
            return Task.CompletedTask;
        };
        await crawler.BeginCrawling();
        crawler.Completed.Should().Be(2);
        crawler.Total.Should().Be(2);
        invokes.Should().Be(2);
        scraped.Select(x => x.Uri).Should().Contain(new Uri(baseAddress, "index.html"));
        scraped.Select(x => x.Uri).Should().Contain(page2Uri);
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
        await crawler.BeginCrawling();
        crawler.Completed.Should().Be(2);
        crawler.Total.Should().Be(2);
    }
    
    
    [Test]
    public async Task CrawlOnlyOnceRelativePath()
    {
        var baseAddress = new Uri("https://localhost");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, 
            HttpClientConfig.Create(baseAddress, response => response.Content = new StringContent("<html><body><a href=\"pages/page2.html\"></a></body></html>", Encoding.UTF8, "text/html")),
            HttpClientConfig.Create(new Uri(baseAddress, "pages/page2.html"), response => response.Content = new StringContent("<html><body><a href=\"../index.html\"></a></body></html>", Encoding.UTF8, "text/html")));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var crawler = new Crawler(scraper, CancellationToken.None);
        await crawler.BeginCrawling();
        crawler.Completed.Should().Be(2);
        crawler.Total.Should().Be(2);
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
        crawler.Scraped += (entity) =>
        {
            htmlPage = entity;
            return Task.CompletedTask;
        };
        await crawler.BeginCrawling();
        htmlPage.Should().NotBeNull();
        htmlPage.Uri.AbsolutePath.Should().Be("/index.html");
    }
    
    
    [Test]
    public async Task ScrapeFailed()
    {
        var baseAddress = new Uri("https://localhost");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, HttpClientConfig.Create(baseAddress, "", HttpStatusCode.NotFound));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var crawler = new Crawler(scraper, CancellationToken.None);
        var invokes = 0;
        crawler.StatusChanged += () => invokes++;
        
        await crawler.BeginCrawling();

        invokes.Should().Be(1);
        crawler.Completed.Should().Be(1);
        crawler.Failed.Should().Be(1);
        crawler.Total.Should().Be(1);
    }
    
}