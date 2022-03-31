using SQFUSystem;


class Program
{
    private static SQFUInOut sqfu;
    private static void  Main()
    {
        sqfu = new SQFUInOut("sempleTest.sqfu");
        PrintSQFU();
        sqfu.ExecuteState();
        PrintSQFU();
    }


    private static void PrintSQFU()
    {
        Console.WriteLine($"Current State -- {sqfu.CurrentState}");
        Console.WriteLine();
        foreach(var pair in sqfu.CurrentDialog)
        {
            Console.WriteLine($"{pair.First} tell --- {pair.Second}");
        }
        Console.WriteLine();
        foreach(var pair in sqfu.CurrentThings)
        {
            Console.WriteLine($"Thing - {pair.First}, Count - {pair.Second}");
        }
    
    }
}