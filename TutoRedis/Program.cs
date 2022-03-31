using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using StackExchange.Redis;

namespace TutoRedis
{
    class Program
    {
        static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions{
                EndPoints = {"localhost:6379"}                
            });
        
        static async Task Main(string[] args)
        {
            BenchmarkRunner.Run<TestRedis>();
        }
    }

    public class TestRedis
    {
        static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions{
                EndPoints = {"localhost:6379"}                
            });
        

        [Benchmark]
        public void Run()
        {
            var db = redis.GetDatabase();
            var isInList = db.SetContains("users", new RedisValue("67.10.71.45"));
            if (!isInList)
            {
                System.Threading.Thread.Sleep(1000);
                throw new Exception();
            }
        }
    }
}
