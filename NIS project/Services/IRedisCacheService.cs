using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIS_project.Services
{
    public interface IRedisCacheService
    {
        Task<T> GetAsync<T>(string key);
        Task<T> SetAsync<T>(string key, T value);
        public Task RemoveAsync(string key);
    }
}
