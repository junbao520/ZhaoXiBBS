using System;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Zhaoxi.BBS.Model;
using Zhaoxi.BBS.Interface;
using Zhaoxi.BBS.Model.DTO;
using Zhaoxi.BBS.Service;

namespace Zhaoxi.AglieFramework.BBS.Controllers
{
	public class ReplyController : BaseController
	{
		IReplyService  _replyService;
		public ReplyController(IReplyService replyService)
		{
			_replyService = replyService;
		}

		[HttpPost]
		public bool Set(ReplysInputDto replysInputDto)
		{
			try
			{
				DateTime datetime = DateTime.Now;
				PostReplys reply = new PostReplys
				{
					PostId = replysInputDto.PostId,
					ReplyContent = replysInputDto.Content,
					CreateTime = datetime,
					CreateUserId = replysInputDto.UserId,
				};
				reply.ReplyContent = reply.ReplyContent;
				_replyService.Insert(reply);
				return true;
			}
			catch (System.Exception)
			{
				throw;
			}

		}
		[HttpPut]
		public bool SetUpDwon(UpDownInputDto upDownInput)
		{
			return _replyService.SetUpDwon(upDownInput);
		}
	}
}