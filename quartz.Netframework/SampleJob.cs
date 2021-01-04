using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace quartz.Netframework
{
    public class SampleJob : IJob
    {
        public async Task Execute(Quartz.IJobExecutionContext context)
        {
            Console.WriteLine("start");
            var tasks = new List<Task>();
            for (int i = 0; i < 50; i++)
            {
                tasks.Add(new Task(() => writefile()));
            }

            tasks.ForEach(t => t.Start());

            await Task.WhenAll(tasks.ToArray());
            //Task.WaitAll(tasks.ToArray());
            //Console.WriteLine("all tasks done.");
        }

        private async Task writefile()
        {
            try
            {
                var index = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
                //var index = DateTime.Now.ToString();
                Console.WriteLine("start:" + index);
                Random rand = new Random(DateTime.Now.Second);
                
                
                for (int i = 0; i < 20; i++)
                {
                    using(var client = new HttpClient())
                    {
                        var result = await client.GetAsync("https://google.com");
                        Console.WriteLine("call no" + index + "," + i.ToString() + ": " + result.StatusCode);
                    }
                }
                
                Console.WriteLine("api call ended for index:" + index);

            }
            catch(Exception e)
            {
                var a = 1;
            }
        }
    }
}
