using System.Collections.Concurrent;

public static class NonBlockingConsole
{
    private static BlockingCollection<char[]> blockingCollection = new();

    static NonBlockingConsole()
    {
        Task.Run(() => { 
            while (true)
                Console.Out.Write(blockingCollection.Take());
        });
    }

    public static void Write(char[] value) => blockingCollection.Add(value);
}