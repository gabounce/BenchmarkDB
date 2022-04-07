using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mongo.WebApp.Models;
using Mongo.WebApp.Services;

namespace Mongo.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MongoController : ControllerBase
    {
        private readonly HotlistService _hotlistService;

        public MongoController(HotlistService hotlistService)
        {
            _hotlistService = hotlistService;
        }
        /*
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
        }*/
        
        [HttpGet]
        public async Task<List<Hotlist>> Get() =>
            await _hotlistService.GetAsync();
        

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Hotlist>> Get(string id)
        {
            var hotlist = await _hotlistService.GetAsync(id);

            if (hotlist is null)
            {
                return NotFound();
            }

            return hotlist;
        }

        [HttpGet("load/{n}")]
        public async Task<IActionResult> Load(int n)
        {
            var hotlist = new Hotlist
            {
                TenantId = 2,
                Name = "IPGreyList",
                HotlistRecord = new List<HotlistRecord>()
            };
            await _hotlistService.CreateAsync(hotlist);
            return CreatedAtAction(nameof(Get), new { id = hotlist.Id }, hotlist);
        }
    }
}