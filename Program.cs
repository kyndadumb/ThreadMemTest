namespace ThreadMemTest;
class Program
{
    static void Main(string[] args)
    {
        // variables
        int max_thread_count = 10000;
        int thread_step = 100;
        
        Stopwatch stopwatch = new Stopwatch();
        string csv_file = "results.csv";

        using (StreamWriter csv_writer = new StreamWriter(csv_file))
        {
            csv_writer.WriteLine("ThreadCount,MemoryUsedMB,ExecutionTime");
            
            for (int threads_number = 100; threads_number <= max_thread_count; threads_number += thread_step)
            {
                Thread[] threads = new Thread[threads_number];
                stopwatch.Restart();

                for (int i = 0; i < threads_number; i++)
                {
                    threads[i] = new Thread(ThreadTask);
                    threads[i].Start();
                }

                foreach (Thread thread in threads) thread.Join();

                stopwatch.Stop();

                Process currentProcess = Process.GetCurrentProcess();
                long memoryUsed = currentProcess.WorkingSet64;

                csv_writer.WriteLine($"{threads_number},{memoryUsed / (1024 * 1024)},{stopwatch.Elapsed}");
                Console.WriteLine($"Threads: {threads_number} | Memory: {memoryUsed / (1024 * 1024)} | Time: {stopwatch.Elapsed}");
            }
        }
    }

    static void ThreadTask()
    {
        // create 1mb byte-array
        byte[] mb = new byte[1000000];

        Thread.Sleep(1000);
    }
}
