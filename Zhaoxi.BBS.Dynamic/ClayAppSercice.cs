using Microsoft.AspNetCore.Mvc;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.BBS.Dynamic
{
    [DynamicWebApi]
    public class ClayAppSercice : IDynamicWebApi
    {

        [HttpGet("{id:int}")]
        public string Get(int id)
        {
            return "ok";
        }
    }
}
