using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.BBS.Database
{
	public interface IDbService
	{



		long Insert<T>(T param) where T : class;

		bool Update<T>(T param) where T : class;

		bool Delete<T>(T param) where T : class;

		IEnumerable<T> Query<T>() where T : class;

		IDbConnection GetDbConnection();
	}
}
