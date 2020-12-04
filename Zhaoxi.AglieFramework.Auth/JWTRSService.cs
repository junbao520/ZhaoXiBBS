using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.AglieFramework.Auth
{

	public class JWTRSService : IJWTService
	{
		#region Option注入
		private readonly JWTTokenOptions _JWTTokenOptions;
		public JWTRSService(IOptionsMonitor<JWTTokenOptions> jwtTokenOptions)
		{
			this._JWTTokenOptions = jwtTokenOptions.CurrentValue;
		}
		#endregion



		public string GetToken(CurrentUserModel userModel)
		{
			//string jtiCustom = Guid.NewGuid().ToString();//用来标识 Token
			var claims = new[]
			{
				   new Claim("UserLevel", userModel.UserLevel.ToString()),
					 new Claim("UserNo", userModel.UserNo),
				   new Claim("UserName",userModel.UserName),
			};
			string keyDir = Directory.GetCurrentDirectory();
			if (RSAHelper.TryGetKeyParameters(keyDir, true, out RSAParameters keyParams) == false)
			{
				keyParams = RSAHelper.GenerateAndSaveKey(keyDir);
			}
			var credentials = new SigningCredentials(new RsaSecurityKey(keyParams), SecurityAlgorithms.RsaSha256Signature);

			var token = new JwtSecurityToken(
			   issuer: this._JWTTokenOptions.Issuer,
			   audience: this._JWTTokenOptions.Audience,
			   claims: claims,
			   expires: DateTime.Now.AddMinutes(60),//5分钟有效期
			   signingCredentials: credentials);
			var handler = new JwtSecurityTokenHandler();
			string tokenString = handler.WriteToken(token);
			return tokenString;
		}
	}
}
