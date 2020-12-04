using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhaoxi.AglieFramework.BBS.Filter
{
	public class CustomExceptionFilter : ExceptionFilterAttribute
	{
		// 错误日志拦截器  用特性
		public override void OnException(ExceptionContext context)
		{

			if (!context.ExceptionHandled)
			{
				context.Result = new JsonResult(new ResultRoot<string>()
				{
					Result = ResultType.Faild,
					Msg = context.Exception.Message
				});

			}
		}
	}
}
