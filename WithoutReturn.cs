using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskStudy
{
    static class WithoutReturn
    {
        static Stopwatch stopwatch;
        public static void StartTest()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            TaskMethod("程序开始");
            var t1 = new Task(() => TaskMethod("Task 1"));
            var t2 = new Task(() => TaskMethod("Task 2"));
            t2.Start();
            t1.Start();
            Task.WaitAll(t1, t2);
            TaskMethod("程序执行1111");
            var t3 = Task.Run(() => TaskMethod("Task 3"));
            var t4 = Task.Factory.StartNew(() => TaskMethod("Task 4"));
            //标记为长时间运行任务,则任务不会使用线程池,而在单独的线程中运行。
            Task.WaitAll(t3, t4);
            var t5 = Task.Factory.StartNew(() => TaskMethod("Task 5"),
            TaskCreationOptions.LongRunning);
            #region 常规的使用方式
            TaskMethod("主线程执行业务开始");
            //创建任务
            Task task = new Task(() =>
            {
                Console.WriteLine("使用System.Threading.Tasks.Task执行异步操作.");
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);
                }
            });
            //启动任务,并安排到当前任务队列线程中执行任务(System.Threading.Tasks.TaskScheduler)
            task.Start();
            Console.WriteLine("主线程执行其他处理");
            task.Wait();
            #endregion
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.ReadKey();
        }

        static void TaskMethod(string str)
        {
            Thread.Sleep(500);
            Console.WriteLine("Task name is {0}，Time is:{1}, Current ThreadIs is {2}.", str, stopwatch.ElapsedMilliseconds, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
