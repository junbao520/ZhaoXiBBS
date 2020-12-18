using Quartz;
using System;
using System.Collections;
using System.Threading.Tasks;
using Zhaoxi.AglieFramework.Core;

namespace Job_TaskOne
{
	[PersistJobDataAfterExecution] //下一次执行可以本次的结果
	[DisallowConcurrentExecution] //不让任务在同一段时间内执行,5s,但是5s没有执行完,然后等执行完,在来执行

	// 如果一直没有执行完??? 没有办法,自己再执行的时候,自己搞超时啥的
	// 只发消息--不存在这个问题,消费端,每次只消费一个, 7.5,7.6 只搞大部分, 单独高  
	// 
								   
	public class TaskOne : Abstract_Ijob, IJob
	{
		public override JobModel InitJobModel()
		{
			return new JobModel("deflaut", "deflautgroup", "deflaut", "3/5 * * * * ?", new Hashtable());
		}
		public Task Execute(IJobExecutionContext context)
		{
			return Task.Run(() =>
			{
				Console.WriteLine("******************1111111111111111111******************");
			}
			);
		}
	}
}
