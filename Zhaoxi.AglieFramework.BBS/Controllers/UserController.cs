using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using Zhaoxi.AglieFramework.Auth;
using Zhaoxi.BBS.Interface;
using Zhaoxi.BBS.Model;
using Zhaoxi.BBS.Model.DTO;
using Zhaoxi.BBS.Service;
namespace Zhaoxi.AglieFramework.BBS.Controllers
{
	public class UserController : BaseController
	{
		#region MyRegion
		private ILogger<UserController> _logger = null;
		private IJWTService _iJWTService = null;
		private readonly IConfiguration _iConfiguration;
		IUserService _userService;
		public UserController(ILoggerFactory factory,
			ILogger<UserController> logger,
			IConfiguration configuration
			, IJWTService service, IUserService userService)
		{
			_logger = logger;
			_iConfiguration = configuration;
			_iJWTService = service;
			_userService = userService;
		}
		#endregion

		[HttpPost]
		public bool SetPost(Users users)
		{
			try
			{
				_userService.Insert(users);
				return true;
			}
			catch (System.Exception ex)
			{
				throw;
			}
		}

		[HttpGet("{n}-{p}")]
		public UsersDto Get(string n, string p)
		{
			var userInfo = _userService.GetUser(n, p);

			UsersDto usersOut = new UsersDto()
			{
				Id = userInfo.Id,
				UserName = userInfo.UserName,
				UserLevel = userInfo.UserLevel,
				UserNo = userInfo.UserNo
			};
			usersOut.Token = this._iJWTService.GetToken(new CurrentUserModel()
			{

				UserNo = userInfo.UserNo,
				UserName = userInfo.UserName,
				UserLevel = userInfo.UserLevel
			});
			return usersOut;
		}
		[HttpGet("{id}")]
		public Users Get(int id)
		{
			var userInfo = _userService.GetByID(id);
			return userInfo;
		}
	}
}