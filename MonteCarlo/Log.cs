namespace MonteCarlo;

public class Log
{
    private static bool printLogs = true;
    
    public static void Print(string str, bool log)
    {
        if (printLogs && log)
        {
            Console.Write(str);
        }
    }
    
    public static void Println(string str, bool log)
    {
        if (printLogs && log)
        {
            Console.WriteLine(str);
        }
    }
}