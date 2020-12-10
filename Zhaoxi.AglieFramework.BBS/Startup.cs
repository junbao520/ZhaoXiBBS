using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Panda.DynamicWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.AglieFramework.Auth;
using Zhaoxi.AglieFramework.Core;
using Zhaoxi.BBS.Database;
using Zhaoxi.BBS.Interface;
using Zhaoxi.BBS.Model.Options;
using Zhaoxi.BBS.Service;

namespace Zhaoxi.AglieFramework.BBS
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

			services.AddControllers();

			// 注入动态api
			services.AddDynamicWebApi();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zhaoxi.AglieFramework.BBS", Version = "v1" });
				c.DocInclusionPredicate((docName, description) => true);
			});

			#region jwt校验  HS
			JWTTokenOptions tokenOptions = new JWTTokenOptions();
			Configuration.Bind("JWTTokenOptions", tokenOptions);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					//JWT有一些默认的属性，就是给鉴权时就可以筛选了
					ValidateIssuer = true,//是否验证Issuer
					ValidateAudience = true,//是否验证Audience
					ValidateLifetime = true,//是否验证失效时间
					ValidateIssuerSigningKey = true,//是否验证SecurityKey
					ValidAudience = tokenOptions.Audience,//
					ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),//拿到SecurityKey
				};
			});
			//定义了一个权限策略 
			services.AddAuthorization(options =>
			{
				options.AddPolicy("LevelPolicy",
					policyBuilder => policyBuilder.RequireAssertion(context =>
					int.Parse(context.User.Claims.First(c => c.Type.Equals("UserLevel")).Value) >= 4//UserLevel属性大于3
					));
			});
			#endregion
			#region HS256
			services.AddScoped<IJWTService, JWTHSService>();
			services.Configure<JWTTokenOptions>(this.Configuration.GetSection("JWTTokenOptions"));

			services.Configure<MySqlConnOptions>(this.Configuration.GetSection("MySqlConn"));

			#endregion

			#region MyRegion
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IPostsService, PostsService>();
			services.AddScoped<IPostTypeService, PostTypeService>();
			services.AddScoped<IReplyService, ReplyService>();
			services.AddScoped<IDbService, DbService>();

			
			#endregion

			#region redis
			services.Configure<RedisConnOptions>(this.Configuration.GetSection("RedisConn"));
			// 依赖注入   这个地方不太好 
			services.AddScoped<CacheClientDB, CacheClientDB>();

			services.Configure<MySqlConnOptions>(this.Configuration.GetSection("MySqlConn"));
			services.AddSingleton<DBConnectFactory, DBConnectFactory>();
			#endregion
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

			}


			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zhaoxi.AglieFramework.BBS v1"));

			app.UseRouting();

			#region  JWT
			app.UseAuthentication();//鉴权：解析信息--就是读取token，解密token
			#endregion
			app.UseCors();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
