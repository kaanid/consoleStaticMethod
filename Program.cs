using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppStaticMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<int,string> act2 = (i,flag) =>
            {
                for (var j = 0; j < 2; j++)
                {
                    DoThing.Do(i.ToString() + "_" + j.ToString(), flag);
                }
            };

            for (var i = 0; i < 3;i++)
            {
                //每次i不同
                act2.Invoke(i, "act2.Invoke");

                //i=3
                Task.Factory.StartNew(() =>
                {
                    act2(i, "Task.Factory.StartNew");
                });

                //同步，每次i不同
                act2(i,"sync");

                //每次i不同
                var t = new Task((i2) =>
                {
                    act2((int)i2,"Task.Start");
                }, i);
                t.Start();

                //每次i不同
                new Thread((a)=> { act2((int)a,"Thread.Start"); }).Start(i);

                //i=3
                Task.Run(() =>
                {
                    act2(i, "Task.Run");
                });
            }

            //并行执行，每次i不同
            Parallel.For(0, 3, (i) =>
            {
                act2(i, "Parallel.For");
            });

            //每次i不同
            foreach (var i in Enumerable.Range(0,3))
            {
                Task.Run(() =>
                {
                    act2(i, "Enumerable.Range");

                });
            }

            Console.Read();
        }
    }

    public class DoThing{
        //线程安全
        private static int n=0;

        /// <summary>
        /// 静态方法并发执行
        /// </summary>
        /// <param name="content"></param>
        /// <param name="flag"></param>
        public static void Do(string content,string flag=null)
        {
            Console.WriteLine("{2} ThreadId:{0},content:{1} {3}",Thread.CurrentThread.ManagedThreadId,content,++n, flag);
        }
    }

}
