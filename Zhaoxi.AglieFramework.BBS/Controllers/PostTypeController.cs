using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Zhaoxi.BBS.Interface;
using Zhaoxi.BBS.Model;
using Zhaoxi.BBS.Service;

namespace Zhaoxi.AglieFramework.BBS.Controllers
{
	public class PostTypeController : BaseController
	{
		IPostTypeService _postTypeService;

		public PostTypeController(IPostTypeService postTypeService)
		{
			_postTypeService = postTypeService;
		}

		[HttpGet]
		public List<PostTypes> GetPostType()
		{
			var types = _postTypeService.GetPostType();
			return types;
		}
	}
}