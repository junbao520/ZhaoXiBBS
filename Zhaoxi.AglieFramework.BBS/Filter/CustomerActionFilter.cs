using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhaoxi.AglieFramework.BBS.Filter
{
	/// <summary>
	/// 统一规范
	/// </summary>
	public class CustomerActionFilter : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			var result = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;

			context.Result = new JsonResult(new ResultRoot<object>()
			{
				Result = ResultType.Succeed,
				Data = result
			});
		}
		public override void OnResultExecuted(ResultExecutedContext context)
		{


		}
	}
}
