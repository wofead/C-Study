using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TaskStudy
{
    static class FromAsyncTest
    {
        private delegate string AsynchronousTask(string threadName);

        private static string Test(string threadName)
        {
            Console.WriteLine("Starting...");
            Console.WriteLine("Is thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Thread.CurrentThread.Name = threadName;
            return string.Format("Thread name: {0}", Thread.CurrentThread.Name);
        }

        private static void Callback(IAsyncResult ar)
        {
            Console.WriteLine("Starting a callback...");
            Console.WriteLine("State passed to a callbak: {0}", ar.AsyncState);
            Console.WriteLine("Is thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Console.WriteLine("Thread pool worker thread id: {0}", Thread.CurrentThread.ManagedThreadId);
        }

        public static void StartTest()
        {
            AsynchronousTask d = Test;
            Console.WriteLine("Option 1");
            Task<string> task = Task<string>.Factory.FromAsync(d.BeginInvoke("AsyncTaskThread", Callback, "a delegate asynchronous call"), d.EndInvoke);

            task.ContinueWith(t => Console.WriteLine("Callback is finished, now running a continuation! Result:{0}", t.Result));
            while(!task.IsCompleted)
            {
                Console.WriteLine(task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            Console.WriteLine(task.Status);
            Console.ReadKey();

        }

        public static void StartTestWithoutCallback()
        {
            AsynchronousTask d = Test;
            Console.WriteLine("Option 2");
            Task<string> task = Task<string>.Factory.FromAsync(d.BeginInvoke, d.EndInvoke, "AsyncTaskThread", "a delegate asynchronous call");
            task.ContinueWith(t => Console.WriteLine("Task is completed, now running a continuation! Result: {0}",
                t.Result));
            while (!task.IsCompleted)
            {
                Console.WriteLine(task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            Console.WriteLine(task.Status);
            Console.ReadKey();
        }

        private static async Task<int> test2(object i)
        {
            

            HttpClient client = new HttpClient();
            var a = await client.GetAsync("http://www.baidu.com");
            Task<string> s = a.Content.ReadAsStringAsync();
            Console.WriteLine(s.Result);
            //System.Threading.Thread.Sleep(3000);
            //MessageBox.Show("hello:"+ i);
            //this.Invoke(new Action(() =>
            //{
            //    pictureBox1.Visible = false;
            //}));
            return 0;
        }

        async public static void call()
        {
            //Func<string, string> funcOne = delegate(string s){ return "fff"; };
            object i = 55;
            var t = Task<Task<int>>.Factory.StartNew(new Func<object, Task<int>>(test2), i);
            Console.ReadKey();
        }
    }
}
