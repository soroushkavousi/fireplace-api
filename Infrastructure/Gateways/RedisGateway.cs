using FireplaceApi.Domain.Extensions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Gateways
{
    public class RedisGateway
    {
        private readonly ILogger<RedisGateway> _logger;
        private readonly IDatabase _db;
        private readonly IServer _server;

        public RedisGateway(ILogger<RedisGateway> logger,
            IConnectionMultiplexer redis)
        {
            _logger = logger;
            _db = redis.GetDatabase();
            _server = redis.GetServer(redis.GetEndPoints()[0]);
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            _logger.LogAppInformation(title: "CACHE_INPUT", parameters: new { key });
            var sw = Stopwatch.StartNew();
            string dataString = await _db.StringGetAsync(key);
            var data = dataString.FromJson<T>();
            _logger.LogAppInformation(sw: sw, title: "CACHE_OUTPUT", parameters: new { data });
            return data;
        }

        public async Task SetData<T>(string key, T data, TimeSpan? lifeSpan = null)
        {
            _logger.LogAppInformation(title: "CACHE_INPUT", parameters: new { key, lifeSpan, data });
            var sw = Stopwatch.StartNew();
            var dataString = data.ToJson(ignoreSensitiveLimit: true);
            await _db.StringSetAsync(key, dataString, lifeSpan);
            _logger.LogAppInformation(sw: sw, title: "CACHE_OUTPUT");
        }

        public async Task RemoveData<T>(string key)
        {
            _logger.LogAppInformation(title: "CACHE_INPUT", parameters: new { key });
            var sw = Stopwatch.StartNew();
            await _db.KeyDeleteAsync(key);
            _logger.LogAppInformation(sw: sw, title: "CACHE_OUTPUT");
        }

        public async Task UpdateLifeSpan(string key, TimeSpan lifeSpan)
        {
            _logger.LogAppInformation(title: "CACHE_INPUT", parameters: new { key, lifeSpan });
            var sw = Stopwatch.StartNew();
            await _db.KeyExpireAsync(key, lifeSpan);
            _logger.LogAppInformation(sw: sw, title: "CACHE_OUTPUT");
        }
    }
}
