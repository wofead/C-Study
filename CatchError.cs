using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskStudy
{
    static class CatchError
    {
        static Stopwatch stopwatch;
        public static void StartTest()
        {

            stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                //Func<string, int, int> fun = TaskMethod;
                Task<int> task = new Task<int>(() => TaskMethod("Task 2", 3));
                task.Start();
                //int result = task.GetAwaiter().GetResult();
                int result = task.Result;
                Console.WriteLine("Result: {0}", result);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Task 2 Exception caught: {0}", ex.Message);
            }
            Console.WriteLine("----------------------------------------------");
            Console.ReadKey();
        }
        static int TaskMethod(string name, int seconds)
        {
            Print(name);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            //return 100;
            throw new Exception("Boom!");
        }

        static void Print(string str)
        {
            Console.WriteLine("{0}，Time is:{1}, Current ThreadIs is {2}, Is threadPool: {3}.", str, stopwatch.ElapsedMilliseconds, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
    }

}
