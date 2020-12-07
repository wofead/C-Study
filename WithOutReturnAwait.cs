using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
namespace TaskStudy
{
    static class WithOutReturnAwait
    {
        static Stopwatch stopwatch;
        public static void StartTest()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            Print("程序开始：");
            var t = AsyncFunction();
            //t.Wait();
            Print(" Main Result:" + t.Result);
            //Task.WaitAll(t);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(string.Format("Main:i={0}", i));
            }
            Console.ReadKey();
        }

        async static Task<int> AsyncFunction()
        {
            Print("AsyncFunction开始");
            await Task.Delay(1000);
            Print("异步操作开始");
            int result = 1;
            for (int i = 0; i < 100; i++)
            {
                result *= i;
            }
            Print("Result:" + result);
            Print("异步操作完成");
            return result;
        }

        static void Print(string str)
        {
            Console.WriteLine("{0}，Time is:{1}, Current ThreadIs is {2}.", str, stopwatch.ElapsedMilliseconds, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
