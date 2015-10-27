using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTestConsole.Model
{
    public class UrlLoadGenerator
    {
        public UrlLoadGenerator()
        {
        }

        public UrlLoadGenerator(int time, int count, int concurrentUsers, string url)
        {
            Time = time;
            Count = count;
            ConcurrentUsers = concurrentUsers;
            RawUrl = url;
        }

        public int Time { get; set; }
        public int Count { get; set; }
        public int ConcurrentUsers { get; set; }
        public string RawUrl { get; set; }

        private void DoWork(long index)
        {
            for (int i = 0; i < Count; i++)
            {
                HttpSendRequest.SendData(RawUrl, "GET");
                Thread.Sleep(Time / Count);

                Console.WriteLine("Running.. url: {0}, ThreadId: {1}, count {2}, Concurrent user no.: {3}, Time:{4}", RawUrl, Thread.CurrentThread.ManagedThreadId, i, index, DateTime.Now.ToString());
            }
        }

        public void RunTest()
        {
            var result = Parallel.For(0, ConcurrentUsers,
                new ParallelOptions()
                {
                    MaxDegreeOfParallelism = ConcurrentUsers
                },
                index => DoWork(index));
        }
    }
}
