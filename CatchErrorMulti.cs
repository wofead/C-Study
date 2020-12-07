using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskStudy
{
    static class CatchErrorMulti
    {
        public static void StartTest()
        {
            try
            {
                var t1 = new Task<int>(() => TaskMethod("Task 3", 3));
                var t2 = new Task<int>(() => TaskMethod("Task 4", 2));
                var complexTask = Task.WhenAll(t1, t2);
                var exceptionHandler = complexTask.ContinueWith(t =>
                        Console.WriteLine("Result: {0}", t.Result),
                        TaskContinuationOptions.OnlyOnFaulted
                    );
                t1.Start();
                t2.Start();
                Task.WaitAll(t1, t2);
            }
            catch (AggregateException ex)
            {
                ex.Handle(exception =>
                {
                    Console.WriteLine(exception.Message);
                    return true;
                });
            }
            Console.ReadKey();
        }
        static int TaskMethod(string name, int seconds)
        {
            Console.WriteLine("Task {0} is running on a thread id {1}. Is thread pool thread: {2}",
                name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            throw new Exception(string.Format("Task {0} Boom!", name));
            //return 42 * seconds;
        }
    }


}
