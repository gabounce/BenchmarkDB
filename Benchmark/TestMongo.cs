using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using MongoHotlist;
using RandomNameGeneratorNG;
using StackExchange.Redis;

namespace Benchmark
{
    public class TestMongo
    {
        private HotlistService _hotlistService;
        
        [GlobalSetup]
        public void GlobalSetup()
        { 
            _hotlistService = new HotlistService(new PersonNameGenerator());
        }

        [Benchmark]
        public void RunSetContains()
        {
            var hotlist = _hotlistService.GetRecordAsync(Constants.HotlistName, "Erik Lojek").Result;
            if (hotlist == null)
            {
                System.Threading.Thread.Sleep(1000);
                throw new Exception();
            }
        }
        
        /*
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
        */
    }
}