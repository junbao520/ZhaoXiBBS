using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zhaoxi.BBS.Model;
using Zhaoxi.AglieFramework.BBS.Filter;
using Zhaoxi.BBS.Interface;
using Zhaoxi.BBS.Service;
namespace Zhaoxi.AglieFramework.BBS.Controllers
{
	//[CustomExceptionFilter]
	public class PostController : BaseController
	{
		IPostsService _postsService;
		public PostController(IPostsService postsService)
		{
			_postsService = postsService;
		}

		//[CustomerActionFilter]
		//[CustomExceptionFilter]
		[Authorize]//��token����
		/// <summary>
		/// ��ѯ��������
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[CustomerActionFilter]
		public List<Posts> Get()
		{
			var posts = _postsService.GetAll();
			return posts;
		}
		/// <summary>
		/// ��ѯ��������
		/// </summary>
		/// <param name="pid"></param>
		/// <returns></returns>
		[HttpGet("{pid}")]
		 
		public PostInfoDto Get(int pid)
		{
			return _postsService.GetByID(pid);
		}
		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="postInput"></param>
		/// <returns></returns>
		 
		[HttpPost]
		public bool SetPost(PostInputDto postInput)
		{
			try
			{
				DateTime date = DateTime.Now;
				Posts post = new Posts
				{
					PostTitle = postInput.PostTitle,
					PostIcon = "https://bbs.3dmgame.com/static/image/common/folder_new.gif",
					PostTypeId = postInput.PostTypeId,
					PostType = postInput.PostType,
					PostContent = postInput.Content,
					CreateTime = date,
					CreateUserId = postInput.UserId,
					EditTime = date,
					EditUserId = postInput.UserId,
					LastReplyTime = date,
					LastReplyUserId = postInput.UserId,
					Clicks = 1,
					Replys = 0,
				};
				_postsService.InsertPost(post);
				return true;
			}
			catch (System.Exception ex)
			{
				throw;
			}
		}

		[HttpPut]
		public bool SetUpDwon(UpDownInputDto upDownInput)
		{
			return _postsService.SetUpDwon(upDownInput);
		}
	}
}