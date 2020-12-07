using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskStudy
{
    static class TaskTest
    {
        static Stopwatch stopwatch;
        public static void StartTest()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            Print("程序开始");
            Task t = new Task(() =>
            {
                Print("任务开始工作");
                //模拟工作过程
                Thread.Sleep(2000);
            });
            t.Start();
            t.ContinueWith((task) =>
            {
                Print("任务完成");
                Console.WriteLine("IsCanceled={0}\tIsCompleted={1}\tIsFaulted={2}, Time is {3}.", task.IsCanceled, task.IsCompleted, task.IsFaulted, stopwatch.ElapsedMilliseconds);
            });
            //
            Print("任务状态" + t.Status);
            Console.ReadKey();
        }

        static void Print(string str)
        {
            Console.WriteLine("{0}，Time is:{1}, Current ThreadIs is {2}.", str, stopwatch.ElapsedMilliseconds, Thread.CurrentThread.ManagedThreadId);
        }

    }
}
