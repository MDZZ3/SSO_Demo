using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Service.Services.RedisService
{
    public class RedisService:IRedisService
    {
        private readonly ConnectionMultiplexer _connection;

        private readonly IDatabase _redis;

        public RedisService(ConnectionMultiplexer multiplexer)
        {
            _connection = multiplexer;
            _redis = _connection.GetDatabase();
        }

        public async Task<string> GetStringAsync(string key)
        {
            string value = await _redis.StringGetAsync(key);
            return value;
        }

        public async Task SetStringAsync(string key,string value)
        {
            await _redis.StringSetAsync(key, value);
        }
    }
}
