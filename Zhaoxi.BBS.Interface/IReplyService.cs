using Zhaoxi.BBS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.BBS.Model;

namespace Zhaoxi.BBS.Interface
{
	public interface IReplyService
	{
		bool Insert(PostReplys replysInputDto);
		bool SetUpDwon(UpDownInputDto upDownInput);
	}
}
