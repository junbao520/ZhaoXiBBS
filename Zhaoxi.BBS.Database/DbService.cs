using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Zhaoxi.BBS.Database
{
	public class DbService : IDbService
	{
		private string connectionStr;

		private readonly DBConnectionFactory _dBConnectionFactory;
		public DbService(DBConnectionFactory dBConnectionFactory)
		{
			_dBConnectionFactory = dBConnectionFactory;
		}

		//public DbService()
		//{
		//	//Microsoft.Extensions.Configuration
		//	//Microsoft.Extensions.Configuration.Binder
		//	//Microsoft.Extensions.Configuration.EnvironmentVariables
		//	//Microsoft.Extensions.Configuration.Json
		//	var builder = new ConfigurationBuilder()
		//	.SetBasePath(Directory.GetCurrentDirectory())
		//	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		//	.AddEnvironmentVariables();
		//	IConfigurationRoot configuration = builder.Build();
		//	connectionStr = configuration.GetSection("mysqlconn").Value.ToString();
		//	//connectionStr = "Database=MyBBSDb;Data Source=47.95.2.2;User Id=root;Password=123456;CharSet=u1tf8;port=13306";
		//	//connectionStr=Configuration["mysqlconn"].to
		//}

		public long Insert<T>(T param) where T : class
		{
			using (IDbConnection con = _dBConnectionFactory.GetDbConnection(ConnType.Write))
			{
				try
				{

					return con.Insert<T>(param);
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}

		//public long InsertBatch<T>(List<T> param) where T : class
		//{
		//	using (IDbConnection con = _dBConnectionFactory.GetDbConnection(ConnType.Write))
		//	{
		//		var tran = con.BeginTransaction();
		//		try
		//		{

		//			return con.Insert<T>(param);
		//		}
		//		catch (Exception ex)
		//		{
		//			throw;
		//		}
		//	}
		//}

		public bool Update<T>(T param) where T : class
		{
			using (IDbConnection con = _dBConnectionFactory.GetDbConnection(ConnType.Write))
			{
				try
				{
					
					return con.Update<T>(param);
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}
		public bool Delete<T>(T param) where T : class
		{
			using (IDbConnection con = _dBConnectionFactory.GetDbConnection(ConnType.Write))
			{
				try
				{
					return con.Delete<T>(param);
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}

		public IEnumerable<T> Query<T>() where T : class
		{
			using (IDbConnection con = _dBConnectionFactory.GetDbConnection(ConnType.Read))
			{
				try
				{
					IEnumerable<T> queryList = con.GetAll<T>();
					return queryList;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		public IDbConnection GetDbConnection()
		{
			return _dBConnectionFactory.GetDbConnection(ConnType.Write);
		}
	}
}
