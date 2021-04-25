using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace TestRedisSpeed
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run);
        }

        private static void Run(Options options)
        {
            var redis = ConnectionMultiplexer.Connect($"{options.Host}:{options.Port}");
            var db = redis.GetDatabase(options.DbNumber);

            var endPoints = redis.GetEndPoints();
            var server = redis.GetServer(endPoints[0]);
            var keys = server.Keys(0, "*");
            // if (options.Debug)
            // {
            //     foreach (var key in keys)
            //     {
            //         Console.WriteLine($"key:{key}, ValueTask: {db.StringGet(key)}");
            //     }
            // }

            
            DateTime start = DateTime.Now;
            var tasks = new List<Task>();
            for (var i = 0; i < options.Threads; i++)
            {
                var id = i;
                var t = Task.Run(() => RunTest(options, db, id));
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());
            DateTime end = DateTime.Now;
            Console.WriteLine($"Whole test time: {(end - start).TotalMilliseconds}");

            if (!Debugger.IsAttached) return;

            Console.WriteLine("Press enter");
            Console.ReadLine();
        }

        private static void RunTest(Options options, IDatabase db, int id)
        {
            int noOfTests=options.NumberOfTests;
            DateTime start = DateTime.Now;
            for (var i = 0; i < noOfTests; i++)
            {
                if (options.Command != null)
                {
                    var x = db.Execute(options.Command);
                    if (options.Debug)
                    {
                        Console.WriteLine($"{JsonConvert.SerializeObject(x)}");
                    }
                }
                else
                {
                    var key = $"value_{id}";
                    db.StringSet(key, i);
                    var o = db.StringGet(key).ToString();
                    if (o != i.ToString()) Console.WriteLine($"Set {i}, received {o}");
                }
            }

            DateTime end = DateTime.Now;
            Console.WriteLine($"Test count: {noOfTests}, Time: {(end - start).TotalMilliseconds}");
        }
    }
}
