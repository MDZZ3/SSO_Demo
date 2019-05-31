using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Service.Services.RedisService
{
    public interface IRedisService
    {
        Task<string> GetStringAsync(string key);

        Task SetStringAsync(string key, string value);

    }
}
