using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Redis;
namespace Zhaoxi.BBS.UI
{
	class Program
	{
		static void Main(string[] args)
		{
			Random random = new Random();

			for (int i = 0; i < 10; i++)
			{

				Console.WriteLine(random.Next(1,10));
			}

			//// 最大是19位 不同的节点,给不同值
			//SnowflakeTool snowflakeTool = new SnowflakeTool(1);
			//for (int i = 0; i < 100; i++)
			//{
			//	Console.WriteLine(snowflakeTool.nextId());
			//}

			//using (var client = new RedisClient("192.168.3.204", 6379))
			//{

			//	Random random = new Random();
			//	client.Set<string>("a", "a", TimeSpan.FromSeconds(10 + random.Next(1, 5)));

			//	//	var lua = @"local count = redis.call('get',KEYS[1])
			//	//                        if(tonumber(count)>=0)
			//	//                        then
			//	//                            redis.call('INCR',ARGV[1])
			//	//                            return redis.call('DECR',KEYS[1])
			//	//                        else
			//	//                            return -1
			//	//                        end";
			//	//	Console.WriteLine(client.ExecLuaAsString(lua, keys: new[] { "number" }, args: new[] { "ordercount" }));



			//	//}
			//	//性能非常好,没有锁,不好性能,而且是原子性
			//	var lua = @"local count = redis.call('SISMEMBER',KEYS[1],ARGV[1])
   //                      if(tonumber(count)>=0)
   //                    then 
   //                       redis.call('SREM',KEYS[1],ARGV[1])
   //                     else
                         
   //                     end
   //                    redis.call('SADD',KEYS[2],ARGV[1]) ";
			//	Console.WriteLine(client.ExecLuaAsString(lua, keys: new string[] { "BBS:Content:1:Up", "BBS:Content:1:Down" }, args: new string[] { "2" }));
			//	//Console.WriteLine(client.ExecLuaAsString(@"redis.call('set','name','clay6668888')"));
			//	//Console.WriteLine(client.ExecLuaAsString(@"return  redis.call('get','name')"));

			//}
		}
	}
}
