using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskStudy
{
    static class SpinLockTest
    {
        public static void StartTest()
        {
            int count = 0;
            Task[] taskList = new Task[10];
            Stopwatch sp = new Stopwatch();
            sp.Start();
            // 不要意外复制。每个实例都是独立的。 因为SpinLock是值类型
            SpinLock _spinLock = new SpinLock();
            for (int i = 0; i < taskList.Length; i++)
            {
                taskList[i] = Task.Run(() =>
                {

                    bool _lock = false;
                    for (int j = 0; j < 100000; j++)
                    {
                        _spinLock.Enter(ref _lock);
                        count++;
                        _spinLock.Exit();
                        _lock = false;
                    }
                });
            }

            sp.Stop();
            Task.WaitAll(taskList);
            Console.WriteLine($"完成! 耗时:{sp.ElapsedTicks}");
            Console.WriteLine($"结果:{count}");
            Console.ReadKey();
        }
    }
}
