using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zhaoxi.AglieFramework.Core;

// WEBPAI   夸平台,夸语言,,  定时作业的任务太重,高耦合  webapi mq --按时发消息, 性能没有,低耦合,如果有页面,维护dll ,或者不要维护dll,直接redis,把dll直接放到redis ,topic ,一个执行时间//
namespace Zhaoxi.AglieBBS.WorkerService
{
	// 任务管理类结束
	public static class SchedulerCenter
	{
		public static StdSchedulerFactory factory = new StdSchedulerFactory();

		public static IScheduler scheduler = factory.GetScheduler().Result;

		public static Task task = scheduler.Start();
		//记录当前我们平台里面所有的执行的任务,uuid
		// 热插拔
		public static List<string> jobs = new List<string>();
		
		// 平台执行的任务的具体对象

		public static Dictionary<string, Abstract_Ijob> dll = new Dictionary<string, Abstract_Ijob>();

		static SchedulerCenter()
		{

		}

		public static void AddJob(Abstract_Ijob abstract_Ijob)
		{

			var jobmodel = abstract_Ijob.InitJobModel();
			// 默认告诉,必须要继承ijob 
			//key --uuid
			IJobDetail job = JobBuilder.Create(abstract_Ijob.GetType()).WithIdentity(abstract_Ijob.InitJobModel().Name, abstract_Ijob.InitJobModel().GroupName).Build();
			//uuid
			var key = new JobKey(jobmodel.Name);
			//加了一个任务
			job.JobDataMap.Add("data", jobmodel.Data);
			//触发器
			ITrigger trigger =
						TriggerBuilder.Create()
							 .StartAt(DateTime.Now)//什么时候开始执行
							 .WithIdentity(jobmodel.Name, jobmodel.GroupName)
							 .WithDescription(jobmodel.Description)
							 .WithCronSchedule(jobmodel.CronSchedule)//表达式
							 .Build();

			// 自己业务,维护当前执行任务--界面展示,可以使用
			jobs.Add(key.Group + "@" + key.Name);

			//吧当前任务加入到我们的执行计划里面
			scheduler.ScheduleJob(job, trigger);

			//var groupNames = scheduler.GetJobGroupNames().Result;
			//foreach (var groupName in groupNames.OrderBy(t => t))
			//{
			//	var jobs = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));

			//}
		}

		public static void RomverJob(Abstract_Ijob abstract_Ijob)
		{
			var jobmodel = abstract_Ijob.InitJobModel();
			// uuid
			var key = new JobKey(jobmodel.Name, jobmodel.GroupName);

			if (scheduler.CheckExists(key).Result)
			{
				// 先暂停,然后删除
				scheduler.PauseJob(key,CancellationToken.None).Wait();
				//涉及到哪些东西,模型,想,会不会边,会不会增加,会不会变 --模型要不要改变--模型--架构--你就按照他干
				scheduler.DeleteJob(key).Wait();
			}

			//var groupNames = scheduler.GetJobGroupNames().Result;
			//foreach (var groupName in groupNames.OrderBy(t => t))
			//{
			//	var jobs = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));

			//}
			// 自己维护,为了以后界面展示
			jobs.Remove(key.Group + "@" + key.Name);
		}


	}
}
