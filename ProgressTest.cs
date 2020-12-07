using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskStudy
{
    static class ProgressTest
    {
        public static void StartTest()
        {
            Task task = Display();
            task.Wait();
            Console.ReadKey();
        }

        static void DoProcessing(IProgress<int> progress)
        {
            for (int i = 0; i < 101; i++)
            {
                Thread.Sleep(100);
                if (progress != null)
                {
                    progress.Report(i);
                }
            }
        }

        static async Task Display()
        {
            var progress = new Progress<int>(percent =>
            {
                Console.Clear();
                Console.Write("{0}%", percent);
            });
            //线程池线程
            await Task.Run(() => DoProcessing(progress));
            Console.WriteLine("");
            Console.WriteLine("结束！！");
        }
    }
}
