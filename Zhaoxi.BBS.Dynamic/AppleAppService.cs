using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Zhaoxi.BBS.Dynamic
{
	public class UpdateAppleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    //// [Authorize]
    //[DynamicWebApi]
    //public class AppleAppService : IDynamicWebApi
    public class AppleAppService : IApplicationService
    {
        #region MyRegion
        private ILogger<AppleAppService> _logger = null;
        private readonly IConfiguration _iConfiguration;
        public AppleAppService(ILogger<AppleAppService> logger,
            IConfiguration configuration)
        {
            this._logger = logger;
            this._iConfiguration = configuration;
        }
        private static readonly Dictionary<int, string> Apples = new Dictionary<int, string>()
        {
            [1] = "Big Apple",
            [2] = "Small Apple"
        };
        #endregion

      
        [AllowAnonymous]
        public async Task UpdateAppleAsync(UpdateAppleDto dto)
        {
            this._logger.LogWarning($"{nameof(AppleAppService)}.{nameof(UpdateAppleAsync)} Invoke....");
            await Task.Run(() =>
            {
                if (Apples.ContainsKey(dto.Id))
                {
                    Apples[dto.Id] = dto.Name;
                }
            });

        }

        /// <summary>
        /// Get An Apple.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [Authorize]//有token就行
        public string Get(int id)
        {
            this._logger.LogWarning($"{nameof(AppleAppService)}.{nameof(Get)}(int id) Invoke....");
            if (Apples.ContainsKey(id))
            {
                return Apples[id];
            }
            else
            {
                return "No Apple!";
            }
        }

        ///// <summary>
        ///// Get  All Apple Async.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IEnumerable<string> GetAllAsync()
        //{
        //    return Apples.Values;
        //}

        /// <summary>
        /// Get All Apple.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "LevelPolicy")]//必须满足策略
        [HttpGet]
        public IEnumerable<string> Get()
        {
            this._logger.LogWarning($"{nameof(AppleAppService)}.{nameof(Get)}() Invoke....");
            return Apples.Values;
        }

        ///// <summary>
        ///// Get All Apple.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IEnumerable<string> GetBigApple()
        //{
        //    this._logger.LogWarning($"{nameof(AppleAppService)}.{nameof(GetBigApple)}() Invoke....");
        //    return Apples.Values;
        //}
        /// <summary>
        /// Update Apple
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost]
        public void Update(UpdateAppleDto dto)
        {
            this._logger.LogWarning($"{nameof(AppleAppService)}.{nameof(Update)}(UpdateAppleDto dto) Invoke....");
            if (Apples.ContainsKey(dto.Id))
            {
                Apples[dto.Id] = dto.Name;
            }
        }

        /// <summary>
        /// Delete Apple
        /// </summary>
        /// <param name="id">Apple Id</param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            this._logger.LogWarning($"{nameof(AppleAppService)}.{nameof(Delete)}(int id) Invoke....");
            if (Apples.ContainsKey(id))
            {
                Apples.Remove(id);
            }
        }

    }
}