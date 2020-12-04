using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.BBS.Database;
using Zhaoxi.BBS.Interface;
using Zhaoxi.BBS.Model;

namespace Zhaoxi.BBS.Service
{
	public class PostTypeService: IPostTypeService
	{
		IDbService dbService;
		public PostTypeService(IDbService dbService)
		{
			this.dbService = dbService;
		}
		public List<PostTypes> GetPostType()
		{
			var types = dbService.Query<PostTypes>().ToList();
			return types;
		}
	}
}
