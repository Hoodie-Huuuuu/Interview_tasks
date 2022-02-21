using System;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            FailProcess();
        }
        catch { }

        Console.WriteLine("Failed to fail process!");
        Console.ReadKey();
    }

    static void FailProcess()
    {
        //1 way
        //System.Diagnostics.Process.GetCurrentProcess().Kill();

        //2 way
        Environment.Exit(0);

        //3 way
        //System.Threading.Thread thread = System.Threading.Thread.CurrentThread;
        //thread.Abort();
    }

}