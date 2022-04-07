using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Postgres.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostgresController : ControllerBase
    {
        private const string ConnectionString = "User ID=testuser;Password=Password00;Host=localhost;Port=5432;Database=testdb;";
        
        [HttpGet("contains/{name}")]
        public async Task<IActionResult> Contains(string name)
        {
            string result;
            await using (var connection = new NpgsqlConnection(ConnectionString))
            {
                result = connection.QueryFirstOrDefault<string>($"SELECT name FROM users where hotlistid = 1 and name = '{name}'");
            }
            return Ok(!string.IsNullOrEmpty(result));
        }
        
        [HttpGet("containsWildCard/{name}")]
        public async Task<IActionResult> ContainsWildCard(string name)
        {
            string result;
            await using (var connection = new NpgsqlConnection(ConnectionString))
            {
                result = connection.QueryFirstOrDefault<string>($"SELECT name FROM users where hotlistid = 1 and name LIKE '%{name}%'");
            }
            return Ok(!string.IsNullOrEmpty(result));
        }
        
        [HttpGet("fuzzy1/{name}")]
        public async Task<IActionResult> Fuzzy1(string name)
        {
            IEnumerable<string> result;
            await using (var connection = new NpgsqlConnection(ConnectionString))
            {
                result = connection.Query<string>($"SELECT name FROM users WHERE hotlistid = 1 and SIMILARITY(name, '{name}') > 0.4 ;");
            }
            return Ok(result);
        }

        [HttpGet("load/{n}")]
        public async Task<IActionResult> Load(int n)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync("DELETE FROM users");
                for (var i = 0; i < n; i++)
                {
                    await connection.ExecuteAsync($"INSERT INTO users VALUES({1}, '{Faker.NameFaker.Name()}')");
                }
            }
            return Ok();
        }
    }
}