using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Zhaoxi.AglieFramework.BBS
{
	public class ResultRoot<T>
	{
		public T Data { get; set; }
		public ResultType Result { get; set; }
		public string Msg { get; set; }
	}

	public enum ResultType
	{
		Succeed = 0,
		Faild = 1,
		NoAuth = 2,
		Other = 3
	}
}
