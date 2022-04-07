using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace Benchmark
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // BenchmarkRunner.Run<TestRedis>();
            // BenchmarkRunner.Run<TestPostgres>();
            BenchmarkRunner.Run<TestMongo>();
        }
    }
}