using Zhaoxi.BBS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Zhaoxi.BBS.Model;

namespace Zhaoxi.BBS.Interface
{
	public interface IPostsService
	{

		List<Posts> GetAll();
		PostInfoDto GetByID(int pid);
		bool InsertPost(Posts post);
		bool SetUpDwon(UpDownInputDto upDownInput);

	}


}
