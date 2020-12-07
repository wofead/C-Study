using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
namespace TaskStudy
{
    static class AcyncAndChild
    {
        static Stopwatch stopwatch;
        static ConcurrentStack<int> stack;
        public static void StartTest()
        {
            stopwatch = new Stopwatch();
            stack = new ConcurrentStack<int>();
            stopwatch.Start();
            //t1和t2串行
            var t1 = Task.Factory.StartNew(() => {
                Print("Task1");
                Thread.Sleep(100);
                stack.Push(1);
                stack.Push(2);
            });

            var t2 = t1.ContinueWith(t =>
            {
                stack.TryPop(out int result);
                Thread.Sleep(100);
                Print("Task 2 pop result:" + result);
            });
            //t2 和 t3并行
            var t3 = t1.ContinueWith(t =>
            {
                stack.TryPop(out int result);
                Thread.Sleep(50);
                Print("Task 3 pop result:" + result);
            });

            Task<string[]> parent = new Task<string[]>(state =>
            {
                Console.WriteLine(state);
                string[] result = new string[2];
                //创建并启动子任务
                new Task(() => { result[0] = "我是子任务1。"; Print("子任务1"); }, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => { result[1] = "我是子任务2。"; Print("子任务2"); }, TaskCreationOptions.AttachedToParent).Start();
                return result;
            }, "我是父任务，并在我的处理过程中创建多个子任务，所有子任务完成以后我才会结束执行。");
            //任务处理完成后执行的操作
            parent.ContinueWith(t =>
            {
                Print("父任务完成！！！");
            });
            //启动父任务
            parent.Start();
            //等待任务结束 Wait只能等待父线程结束,没办法等到父线程的ContinueWith结束
            //parent.Wait();

            Print("Main");
            Console.ReadKey();
            
        }


        static void Print(string str)
        {
            Console.WriteLine("{0}，Time is:{1}, Current ThreadIs is {2}.", str, stopwatch.ElapsedMilliseconds, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
