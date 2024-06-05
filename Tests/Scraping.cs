using ArrangeDependencies.Autofac;
using ArrangeDependencies.Autofac.HttpClient;
using Castle.Core.Smtp;
using FluentAssertions;

namespace Tests;

[TestFixture]
public class Scraping
{
    [Test]
    public async Task Scrape()
    {
        var baseAddress = new Uri("https://localhost");
        
        var arrange = Arrange.Dependencies(dependencies =>
        {
            dependencies.UseHttpClientFactory(client => client.BaseAddress = baseAddress, HttpClientConfig.Create(baseAddress, "<html></html>"));
        });

        var httpClient = arrange.Resolve<IHttpClientFactory>().CreateClient();
        var scraper = new Scraper(httpClient);
        var calls = 0;
        scraper.OnSuccess +=  (htmlPage) =>
        {
            calls++;
            return Task.CompletedTask;
        };
        await scraper.Scrape("", CancellationToken.None);

        calls.Should().Be(1);
    }
}