using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NIS_project.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = _cache.GetString(key);

            if (value != null)
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        public async Task<T> SetAsync<T>(string key, T value)
        {
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                SlidingExpiration = TimeSpan.FromMinutes(60)
            };

            Console.WriteLine("not here: " + value);
            _cache.SetString(key, JsonSerializer.Serialize<T>(value), timeOut);
            Console.WriteLine("This bastard: " + JsonSerializer.Serialize(value));

            return value;
        }

        public async Task RemoveAsync(string key) 
        {
            _cache.Remove(key);

            //Because of lack of time this id how cache is updated
            //This method is called by Create() and Update() repository methods as well in
            //order to remove old data
            _cache.Remove("AllCars");
            _cache.Remove("AllOwners");
            _cache.Remove("AllEngines");
            _cache.Remove("AllManufacturers");
        }
    }
}
