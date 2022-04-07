using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoHotlist;
using MongoHotlist.Models;

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

        [HttpGet("contains/{recordValue}")]
        public async Task<IActionResult> Contains(string recordValue)
        {
            var result = await _hotlistService.GetRecordAsync(Constants.HotlistName, recordValue);
            return Ok(result != null);
        }

        [HttpGet("load/{n}")]
        public async Task<IActionResult> Load(int n)
        {
            await _hotlistService.RemoveAsync(Constants.HotlistName);
            var hotlist = new Hotlist
            {
                TenantId = 1,
                Name = Constants.HotlistName,
                HotlistRecord = new List<HotlistRecord>()
            };
            await _hotlistService.CreateAsync(hotlist);
            for (var i = 0; i < n; i++)
            {
                await _hotlistService.CreateRecordAsync(Constants.HotlistName);
            }

            return Ok();
        }
    }
}