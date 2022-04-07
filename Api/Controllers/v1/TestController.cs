using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    public class TestController : BaseController
    {
        [HttpGet]
        [Route("GetFullName")]
        public async Task<string> GetFullName()
        {
            return "Cavid Bashirov";
        }
    }
}
