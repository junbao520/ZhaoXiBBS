using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace QuartZDemo
{
	public class Test : IJob
	{
		public System.Threading.Tasks.Task Execute(IJobExecutionContext context)
		{
			return Task.Run(() =>
			{

				Console.WriteLine("哈哈哈 你好帅");
			})
;
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			// 搞一个定时作业,部署一个项目,搞一个,部署一个项目,搞一个部署
			//平台可以管理所有的定时作业 dll 上传上去,然后不用你管 
			// 定时做也什么事 DLL 
			// 定时作业的平台

			//需要哪些东西
			//  任务名称,和任务组,我是需要,通过两个来判断任务唯一性
			// Cron 表达式, 到时间执行的方法 IJob
			IScheduler scheduler = new StdSchedulerFactory().GetScheduler().Result;
			IJobDetail testJob = JobBuilder.Create<Test>()
					 .WithIdentity("Test1", "group1")
					 .WithDescription("this is Test1")
					 .StoreDurably()
					 .Build();

			ITrigger trigger =
						TriggerBuilder.Create()
							 .StartAt(DateTime.Now)//什么时候开始执行
							 .WithCronSchedule("0/3 * * * * ? ")// 时间表达式
							 .Build();

			scheduler.ScheduleJob(testJob, trigger);
			scheduler.Start();
			Console.WriteLine("Hello World!");
			Console.ReadKey();
		}
	}
}
