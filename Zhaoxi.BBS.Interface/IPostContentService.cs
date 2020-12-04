using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.BBS.Model;

namespace Zhaoxi.BBS.Interface
{
	public interface IPostContentService
	{
		PostsContent GetByID(int id);
		bool InsertPostsContent(PostsContent  postsContent);
	}
}
