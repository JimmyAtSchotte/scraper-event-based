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
    
    public void Print(int completed, int total, int failed)
    {
        if (!Monitor.TryEnter(monitor)) 
            return;

        try
        {
            Console.SetCursorPosition(_cursorLeft, _cursorTop);
            Console.Write($"Scraped {completed} of {total}. Failed: {failed}");
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