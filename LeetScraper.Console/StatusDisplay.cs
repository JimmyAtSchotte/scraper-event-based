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
            var status = crawler.GetStatus();
            Console.SetCursorPosition(_cursorLeft, _cursorTop);
            Console.WriteLine($"Total:\t\t{status.Total}             ");
            Console.WriteLine($"Scraped:\t{status.Completed}         ");
            Console.WriteLine($"Pending:\t{status.Pending}           ");
            Console.WriteLine($"Failed:\t\t{status.Failed}           ");
            
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