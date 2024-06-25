using LeetScraper.Core;

public class StatusDisplay
{
    private static object monitor = new object();
    private int _cursorTop;
    private int _cursorLeft;
    public StatusDisplay(int cursorTop, int cursorLeft)
    {
        _cursorTop = cursorTop;
        _cursorLeft = cursorLeft;
    }
    
    public void Print(Crawler crawler)
    {
        if (!Monitor.TryEnter(monitor)) 
            return;

        try
        {
            Console.SetCursorPosition(_cursorLeft, _cursorTop);
            Console.Write($"Scraped {crawler.Completed} of {crawler.Total}. Failed: {crawler.Failed}");
        }
        finally
        {
            Monitor.Exit(monitor);
        }
    }

    public void MoveNextLine()
    {
        Console.SetCursorPosition(_cursorLeft, _cursorTop+1);
    }
}