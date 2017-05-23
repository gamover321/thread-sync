using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApplication19
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine("Enter any number or 'exit' to stop thread or 'exitp' to exit program");

            ThreadQueue.SetUpAndRun();
            while (true)
            {

                var data = Console.ReadLine() ?? "0";
                if (data.ToLower() == "exit")
                {
                    ThreadQueue.CancellationTokenSource.Cancel();
                }
                else if (data.ToLower() == "exitp")
                {
                    return;
                }
                else
                {
                    var count = Int32.Parse(data);
                    for (var i = 0; i < count; i++)
                    {
                        ThreadQueue.Queue.Enqueue(i);
                    }

                    ThreadQueue.ThreadNotification.Set();
                }
            }

        }


        public class ThreadQueue
        {
            public static volatile Queue<int> Queue = new Queue<int>();

            public static AutoResetEvent ThreadNotification = new AutoResetEvent(false);
            public static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

            public static void SetUpAndRun()
            {
                var thread = new Thread(DoWork);
                thread.Start();
            }

            private static void DoWork()
            {
                Console.WriteLine("Starting DoWork");
                while (!CancellationTokenSource.Token.IsCancellationRequested)
                {
                    if (Queue.Count > 0)
                    {
                        var item = Queue.Dequeue();
                        Console.WriteLine("get {0}", item);
                    }
                    Thread.Sleep(1000);
                    Console.WriteLine("Zzzz...");
                    if (Queue.Count == 0)
                    {
                        ThreadNotification.WaitOne();
                    }
                }
                Console.WriteLine("Ending DoWork");
            }
        }


    }
}
