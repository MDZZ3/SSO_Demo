using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SSO.Service.Services.RedisService
{
   public class RedisConfig
    {
        public string Host { get; set; }

        public string Post { get; set; }

        public string PassWord { get; set; }

        private static Object redis_lock = new object();

        private static ConnectionMultiplexer multiplexer = null;

        public  ConnectionMultiplexer Create()
        {
            if (multiplexer == null)
            {
                lock (redis_lock)
                {
                    if (multiplexer == null)
                    {
                        String connectstring = $"{Host}:{Post},password={PassWord}";
                        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(connectstring);
                        //防止单例出问题，代码来源：CLR via C#著名的双检锁技术章节
                        Volatile.Write(ref multiplexer, connectionMultiplexer);
                    }
                }
            }
            return multiplexer;
        }
    }
}
