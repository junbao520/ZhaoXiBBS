using Quartz;
using System;
using System.Collections;
using System.Threading.Tasks;
using Zhaoxi.AglieFramework.Core;

namespace Job_TaskTwo
{
	[PersistJobDataAfterExecution] //下一次执行可以本次的结果
	[DisallowConcurrentExecution] //不让任务在同一段时间内执行
	public class TaskTwo : Abstract_Ijob, IJob
	{

		public override JobModel InitJobModel()
		{

			return new JobModel("abc", "deflautgroup2", "deflaut2", "3/10 * * * * ?", new Hashtable() { { "key1", "" }, { "key2", "" } });
		}
		public Task Execute(IJobExecutionContext context)
		{
			var ht = context.JobDetail.JobDataMap.Get("data") as Hashtable;


			foreach (var item in ht.Keys)
			{
				Console.WriteLine(item);
			}
			return Task.Run(() =>
			{
				Console.WriteLine("******************3333333333333******************");
			}
			);
		}
	}
}
