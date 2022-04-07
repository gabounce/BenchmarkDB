using System;
using System.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;
using Dapper;
using Faker;
using Npgsql;

namespace Benchmark
{
    public class TestPostgres
    {
        private NpgsqlConnection _connection;
        private const string ConnectionString = "User ID=testuser;Password=Password00;Host=localhost;Port=5432;Database=testdb;";
        
        [GlobalSetup]
        public void GlobalSetup()
        {
            _connection = new NpgsqlConnection(ConnectionString);
        }
        
        [Benchmark]
        public void RunSetContains()
        {
            var name = _connection.QueryFirst<string>("SELECT name FROM users where hotlistid = 1 and name = 'Deborah Davies'");
            if (string.IsNullOrEmpty(name))
            {
                Thread.Sleep(1000);
                throw new Exception();
            }
        }
        
        //[Benchmark]
        public void RunSetContainsWildCard()
        {
            var names = _connection.Query<string>("SELECT name FROM users where hotlistid = 1 and name LIKE '%rah%'");
            if (!names.Any())
            {
                Thread.Sleep(1000);
                throw new Exception();
            }
        }
        
        //[Benchmark]
        public void RunSetContainsFuzzy1()
        {
            var names = _connection.Query<string>("SELECT name FROM users WHERE hotlistid = 1 and SIMILARITY(name, 'Debora Daviz') > 0.4 ;");
            if (!names.Any())
            {
                Thread.Sleep(1000);
                throw new Exception();
            }
        }
        
        //[Benchmark]
        public void RunSetAdd()
        {
            _connection.Execute($"INSERT INTO users VALUES (1, '{NameFaker.Name()}')");
        }

        [GlobalCleanup]
        public void GlobalCleanUp()
        {
            _connection.Dispose();
        }
    }
}