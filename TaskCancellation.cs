using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskStudy
{
    static class TaskCancellation
    {
        static Stopwatch stopwatch;
        public static void StartTest()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;
            ManualResetEvent mre = new ManualResetEvent(true);
            Task task = new Task(() =>
            {
                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }

                    // 初始化为true时执行WaitOne不阻塞
                    mre.WaitOne();
                    Print("Task");
                    // Doing something.......
                    // 模拟等待100ms
                    Thread.Sleep(500);
                    //await Task.Delay(500);
                }
            }, ct);
            task.Start();
            Print(task.Status.ToString());
            bool isReset = false;
            bool isSet = false;
            bool isCancel = false;
            while (true)
            {
                if (stopwatch.ElapsedMilliseconds > 2000 && stopwatch.ElapsedMilliseconds <= 4000)
                {
                    if (!isReset)
                    {
                        isReset = true;
                        Print(task.Status.ToString());
                        Print("暂停");
                        mre.Reset();
                    }
                }
                else
                {
                    if (stopwatch.ElapsedMilliseconds > 4000 && stopwatch.ElapsedMilliseconds <= 6000)
                    {
                        if (!isSet)
                        {
                            isSet = true;
                            Print(task.Status.ToString());
                            Print("继续");
                            mre.Set();
                        }
                    }
                    else
                    {
                        if (stopwatch.ElapsedMilliseconds > 6000)
                        {
                            if (!isCancel)
                            {
                                isCancel = true;
                                Print(task.Status.ToString());
                                Print("取消");
                                cts.Cancel();
                            }
                        }
                    }
                }
                if (task.Status == TaskStatus.Canceled || task.Status == TaskStatus.Faulted || task.Status == TaskStatus.RanToCompletion)
                {
                    Print("结束:" + task.Status);
                    break;
                }
            }
            Print("Finish");
            Console.ReadLine();
        }

        static void Print(string str)
        {
            Console.WriteLine("{0}，Time is:{1}, Current ThreadIs is {2}.", str, stopwatch.ElapsedMilliseconds, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
//WaitingToRun，Time is:17, Current ThreadIs is 1.
//Task，Time is:22, Current ThreadIs is 3.
//Task，Time is:533, Current ThreadIs is 3.
//Task，Time is:1047, Current ThreadIs is 3.
//Task，Time is:1549, Current ThreadIs is 3.
//Running，Time is:2001, Current ThreadIs is 1.
//暂停，Time is:2001, Current ThreadIs is 1.
//Running，Time is:4001, Current ThreadIs is 1.
//继续，Time is:4001, Current ThreadIs is 1.
//Task，Time is:4001, Current ThreadIs is 3.
//Task，Time is:4509, Current ThreadIs is 3.
//Task，Time is:5017, Current ThreadIs is 3.
//Task，Time is:5527, Current ThreadIs is 3.
//Running，Time is:6001, Current ThreadIs is 1.
//取消，Time is:6001, Current ThreadIs is 1.
//结束:RanToCompletion，Time is:6033, Current ThreadIs is 1.
//Finish，Time is:6033, Current ThreadIs is 1.