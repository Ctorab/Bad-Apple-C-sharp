using System.Collections.Concurrent;

public static class NonBlockingConsole
{
    private static BlockingCollection<char[]> blockingCollection = new();

    static NonBlockingConsole()
    {
        var writeThread = new Thread(() =>
        {
            Task.Run(() =>
            {
                while (true)
                    WriteToConsole();
            });
        });
        
        writeThread.IsBackground = true;
        writeThread.Start();

        Console.WriteLine(writeThread.Name);
    }

    private static void WriteToConsole()
    {
        using (var sw = new StreamWriter(Console.OpenStandardOutput()))
        {
            Console.SetCursorPosition(0, 0);
            sw.Write(blockingCollection.Take());
        }
    }

    public static void Write(char[] value) => blockingCollection.Add(value);
}