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

			// ע�붯̬api
			services.AddDynamicWebApi();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zhaoxi.AglieFramework.BBS", Version = "v1" });
				c.DocInclusionPredicate((docName, description) => true);
			});

			#region jwtУ��  HS
			JWTTokenOptions tokenOptions = new JWTTokenOptions();
			Configuration.Bind("JWTTokenOptions", tokenOptions);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					//JWT��һЩĬ�ϵ����ԣ����Ǹ���Ȩʱ�Ϳ���ɸѡ��
					ValidateIssuer = true,//�Ƿ���֤Issuer
					ValidateAudience = true,//�Ƿ���֤Audience
					ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
					ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
					ValidAudience = tokenOptions.Audience,//
					ValidIssuer = tokenOptions.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),//�õ�SecurityKey
				};
			});
			//������һ��Ȩ�޲��� 
			services.AddAuthorization(options =>
			{
				options.AddPolicy("LevelPolicy",
					policyBuilder => policyBuilder.RequireAssertion(context =>
					int.Parse(context.User.Claims.First(c => c.Type.Equals("UserLevel")).Value) >= 4//UserLevel���Դ���3
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
			// ����ע��   ����ط���̫�� 
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
			app.UseAuthentication();//��Ȩ��������Ϣ--���Ƕ�ȡtoken������token
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
