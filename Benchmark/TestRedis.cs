using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using RandomNameGeneratorNG;
using StackExchange.Redis;

namespace Benchmark
{
    public class TestRedis
    {
        static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions{
                EndPoints = {"localhost:6379"}                
            });
        
        private PersonNameGenerator _nameGenerator;
        private IDatabase _db;
        
        [GlobalSetup]
        public void GlobalSetup()
        {
            _nameGenerator = new PersonNameGenerator();
            _db = redis.GetDatabase();
        }

        //[Benchmark]
        public void RunSetContains()
        {
            var isInList = _db.SetContains("users", new RedisValue("Dwayne Virgil"));
            if (!isInList)
            {
                System.Threading.Thread.Sleep(1000);
                throw new Exception();
            }
        }
        
        //[Benchmark]
        public void RunSetContainsWildCard()
        {
            var redisValues = _db.SetScan("users", new RedisValue("*way*"), 250);
            var results = redisValues.Select(r => r.ToString()).ToArray();
            if (!results.Any())
            {
                System.Threading.Thread.Sleep(1000);
                throw new Exception();
            }
        }
        
        [Benchmark]
        public void RunSetAdd()
        {
            _db.SetAdd("users", new RedisValue(_nameGenerator.GenerateRandomFirstAndLastName()));
        }
    }
}