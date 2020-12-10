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

			// 最大是19位 不同的节点,给不同值
			SnowflakeTool snowflakeTool = new SnowflakeTool(1);
			for (int i = 0; i < 100; i++)
			{
				Console.WriteLine(snowflakeTool.nextId());
			}

			using (var client = new RedisClient("192.168.3.204", 6379))
			{


			}
		}
	}
}
