using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.BBS.Database;
using Zhaoxi.BBS.Model;

namespace Zhaoxi.BBS.Interface
{
	public class PostContentService : IPostContentService
	{
		IDbService dbService;
		public PostContentService(IDbService dbService)
		{
			this.dbService = dbService;
		}
		public PostsContent GetByID(int id)
		{
			return dbService.Query<PostsContent>().Where(m => m.Id == id).FirstOrDefault();
		}

		public bool InsertPostsContent(PostsContent postsContent)
		{
			dbService.Insert(postsContent);
			return true;
		}
	}
}
