using System.IO.Abstractions;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LeetScraper.Core;

namespace LeetScraper.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void StartScraping_OnClick(object sender, RoutedEventArgs e)
    {
        StartScraping.IsEnabled = false;
        
        var uri =  new Uri("https://books.toscrape.com/");

        using var client = new HttpClient();
        client.BaseAddress = uri;

        var fileStorage = new LocalFileStorage(new FileSystem(), uri.Host);
        var scraper = new Scraper(client);
        var crawler = new Crawler(scraper, new CancellationToken());
        crawler.StatusChanged += async () =>
        {
            await Dispatcher.InvokeAsync(() =>
            {
                Completed.Content = $"Completed: {crawler.Completed}";
                Total.Content = $"Total: {crawler.Total}";
                Failed.Content = $"Failed: {crawler.Failed}";
                Progress.Maximum = crawler.Total;
                Progress.Value = crawler.Completed;
            });
        };
        crawler.Scraped += async (entity) => await fileStorage.Store(entity);
        await crawler.BeginCrawling();
        
        StartScraping.IsEnabled = false;
    }
}