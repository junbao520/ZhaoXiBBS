using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.BBS.Model.Options;

namespace Zhaoxi.BBS.Database
{
	public class DBConnectionFactory
	{
		private readonly MySqlConnOptions mySqlConnOptions;
		public DBConnectionFactory(IOptionsMonitor<MySqlConnOptions> jwtTokenOptions)
		{
			this.mySqlConnOptions = jwtTokenOptions.CurrentValue;
		}
		Random Rdm = new Random();
		public IDbConnection GetDbConnection(ConnType connType=ConnType.Write)
		{
			
			IDbConnection dbConnection = null;
			switch (connType)
			{
				case ConnType.Read:
					int iRdm = Rdm.Next(0, 2000) % mySqlConnOptions.Read.Length;
					dbConnection = new MySqlConnection(mySqlConnOptions.Read[iRdm]);
					Console.WriteLine(iRdm);
					break;
				case ConnType.Write:
					dbConnection = new MySqlConnection(mySqlConnOptions.Write);
					break;
				default:
					break;
			}
			return dbConnection;

		}
	}

	//public enum ConnType
	//{
	//	Read = 0,
	//	Write = 1
	//}
}


