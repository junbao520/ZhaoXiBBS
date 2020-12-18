using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zhaoxi.AglieFramework.Core;

namespace Zhaoxi.AglieBBS.WorkerService
{
	public class JobManage
	{

		public void Stop(CancellationToken stoppingToken)
		{
			 


		}
		public async Task RunAsync()
		{
			// 加载dll,做增删改
			List<Abstract_Ijob> abstract_Ijobs = new List<Abstract_Ijob>();
			// netcore 3.1 .net 5 load
			//linux 部署的时候,挂载  /home
			var path = @"D:\345\";
			// dll 必须要是JOB_ 如果不是,我不管,我不加载 --空间 可以不写 
			string[] fileInfos = Directory.GetFiles(path).Where(f => f.Contains("Job_")).ToArray();
			// 通过stream的方式,加载dll ,防止,后面dll删差不了,dll被用了,
			var _AssemblyLoadContext = new AssemblyLoadContext(Guid.NewGuid().ToString("N"), true);

			// 记录当前文件信息和对象信息
			Dictionary<string, Abstract_Ijob> fileInfolist = new Dictionary<string, Abstract_Ijob>();

			foreach (var item in fileInfos)
			{
				Abstract_Ijob Abstract_Ijob;
				using (var fs = new FileStream(item, FileMode.Open, FileAccess.Read))
				{
					// 通过流,直接加载dll,加载完之后,可以删除dll  界面化得管理,吧现在dll删了,吧之前dll复制进来,保持之前版本dll的
					// 如果平台强大,dll 都不要,上传变成stream ,写入redis,数据库// dll 流,加个版本号
					// 读redis ,吧字节变成流
				 
					var assembly = _AssemblyLoadContext.LoadFromStream(fs);
					//var assembly = _AssemblyLoadContext.LoadFromStream(fs);//可以直接读数据库
					// 当前继承ijob的类,这些类才是我们的任务
					Type type = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob))).FirstOrDefault();
					Abstract_Ijob = Activator.CreateInstance(type) as Abstract_Ijob;
				}

				FileInfo fileInfo = new FileInfo(item);

				Abstract_Ijob.FileInfoLastTime = fileInfo.LastWriteTime;
				fileInfolist.Add(fileInfo.Name, Abstract_Ijob);
				// 添加任务  
				if (!SchedulerCenter.dll.ContainsKey(fileInfo.Name))
				{
					SchedulerCenter.AddJob(Abstract_Ijob);
					SchedulerCenter.dll[fileInfo.Name] = Abstract_Ijob;
				}

				//dll 修改
				if (SchedulerCenter.dll.ContainsKey(fileInfo.Name) && fileInfo.LastWriteTime > SchedulerCenter.dll[fileInfo.Name].FileInfoLastTime)
				{
					// 先删 
					SchedulerCenter.RomverJob(Abstract_Ijob);
					SchedulerCenter.dll.Remove(fileInfo.Name);

					// 再加
					SchedulerCenter.AddJob(Abstract_Ijob);
					SchedulerCenter.dll[fileInfo.Name] = Abstract_Ijob;
				}

			}
			// 获取所有的dll 这是已经存在的任务 1,2,3
			var dllkeys = SchedulerCenter.dll.Keys.ToArray();
			//集合是删除,所以需要倒序循环,为了不报错
			for (int i = dllkeys.Length - 1; i >= 0; i--)
			{
				// 移除任务 当前的ll 有没有少 1,2 ,删除不在的dll的任务
				if (!fileInfolist.Keys.Contains(dllkeys[i]))
				{
					//先移除任务
					SchedulerCenter.RomverJob(SchedulerCenter.dll[dllkeys[i]]);
					//业务的dll 
					SchedulerCenter.dll.Remove(dllkeys[i]);
				}
			}



		}
	}
}
