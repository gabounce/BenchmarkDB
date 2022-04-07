using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RandomNameGeneratorNG;
using StackExchange.Redis;

namespace Redis.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
    
        public RedisController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpGet("contains/{key}/{val}")]
        public async Task<IActionResult> Contains(string key, string val)
        {
            var db = _redis.GetDatabase();
            var result = db.SetContains(key, new RedisValue(val));

            return Ok(result);
        }
        
        [HttpGet("scan/{key}/{val}")]
        public async Task<IActionResult> Scan(string key, string val)
        {
            var db = _redis.GetDatabase();
            var redisValues = db.SetScan(key, new RedisValue(val), 250);

            var results = redisValues.Select(r => r.ToString()).ToArray();
            return Ok(results);
        }

        [HttpGet("load/{key}/{n}")]
        public async Task<IActionResult> Load(string key, int n)
        {
            var db = _redis.GetDatabase();
            db.KeyDelete(key);

            var generator = new PersonNameGenerator();
            
            for (var i = 0; i < n; i++)
            {
                db.SetAdd(key, new RedisValue(generator.GenerateRandomFirstAndLastName()));
            }

            return Ok();
        }
    }
}