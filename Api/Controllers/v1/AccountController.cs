using Microsoft.AspNetCore.Mvc;
using Service.Account;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpResource userSignUpResource)
        {
            ServiceResponse response = await _accountService.ResgisterAsync(userSignUpResource);

            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInResource userSignInResource)
        {
            ServiceResponse response = await _accountService.LoginAsync(userSignInResource);

            return Ok(response);
        }
    }
}
