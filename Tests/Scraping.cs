using System.Text;
using ArrangeDependencies.Autofac;
using ArrangeDependencies.Autofac.HttpClient;
using FluentAssertions;
using FluentAssertions.Equivalency;
using LeetScraper;
using LeetScraper.WebEntities;
using File = LeetScraper.WebEntities.File;

namespace Tests;

[TestFixture]
public class Scraping
{
    [Test]
    public async Task ScrapeHtmlPage()
    {
        var baseAddress = new Uri("https://localhost");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, HttpClientConfig.Create(baseAddress, 
            response => response.Content = new StringContent("<html></html>", Encoding.UTF8, "text/html")));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var scraped = default(IWebEntity);
        scraper.OnSuccess +=  (htmlPage) =>
        {
            scraped = htmlPage;
            return Task.CompletedTask;
        };
        await scraper.Scrape("", CancellationToken.None);

        scraped.Should().BeAssignableTo<HtmlPage>();
    }
    
    [Test]
    public async Task ScrapeImages()
    {
        var baseAddress = new Uri("https://localhost");
        var imageAddress = new Uri(baseAddress, "image.jpg");
        var bytes = new Byte[] { 0, 0, 0, 0 };
        
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, 
            
            HttpClientConfig.Create(imageAddress, response => response.Content = new ByteArrayContent(bytes)));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var scraped = default(IWebEntity);
        scraper.OnSuccess +=  (entity) =>
        {
            scraped = entity;
            return Task.CompletedTask;
        };
        await scraper.Scrape("image.jpg", CancellationToken.None);

        scraped.Should().BeAssignableTo<File>();
        scraped.Bytes.Should().BeEquivalentTo(bytes, _ => new EquivalencyAssertionOptions<byte>().WithStrictOrdering());
        scraped.Uri.Should().Be(imageAddress);
    }
}