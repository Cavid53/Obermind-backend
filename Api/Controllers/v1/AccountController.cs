using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Account;
using Service.Common;
using System.ComponentModel.DataAnnotations;
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
            ServiceResponse<string> response = await _accountService.ResgisterAsync(userSignUpResource);

            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInResource userSignInResource)
        {
            ServiceResponse<string> response = await _accountService.LoginAsync(userSignInResource);

            return Ok(response);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody][Required] string[] roleNames)
        {
            ServiceResponse<string> response = await _accountService.CreateRoleAsync(roleNames);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string userEmail, string roleName)
        {
            ServiceResponse<string> response = await _accountService.AddUserToRoleAsync(userEmail, roleName);
            return Ok(response);
        }
    }
}
