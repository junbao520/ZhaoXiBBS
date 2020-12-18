using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Zhaoxi.AglieBBS.WorkerService
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;

		public Worker(ILogger<Worker> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			 
			// 多线程控制,
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					new JobManage().RunAsync().Wait();

					_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

					// 下面是循环时间 多久,自己来
					await Task.Delay(1000, stoppingToken);

				}
				catch (Exception ex)
				{

					throw;
				}

			}
		}
	}
}
