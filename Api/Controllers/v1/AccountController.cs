using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Account;
using Service.Common;
using System.ComponentModel.DataAnnotations;
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

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody][Required] string[] roleNames)
        {
            ServiceResponse response = await _accountService.CreateRoleAsync(roleNames);
            return Ok(response);
        }
    }
}
