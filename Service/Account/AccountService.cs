using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountService(IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ServiceResponse> ResgisterAsync(UserSignUpResource userSignUpResource)
        {
            ServiceResponse _response = new();

            try
            {
                var user = _mapper.Map<UserSignUpResource, AppUser>(userSignUpResource);
                var userCreateResult = await _userManager.CreateAsync(user, userSignUpResource.Password);
                if (userCreateResult.Succeeded)
                {
                    _response.Success = true;
                    _response.Message = "Created";
                    return _response;
                }

                _response.Success = false;
                _response.Error = "Identity error";
                _response.ErrorMessages = new List<string> { Convert.ToString(userCreateResult.Errors.FirstOrDefault().Description) };
                return _response;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        public async Task<ServiceResponse> LoginAsync(UserSignInResource userSignInResource)
        {
            ServiceResponse _response = new();

            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.UserName == userSignInResource.Email);

                if (user is null)
                {
                    _response.Success = false;
                    _response.Message = $"{userSignInResource.Email} - user not found";
                    return _response;
                }

                var userSigninResult = await _userManager.CheckPasswordAsync(user, userSignInResource.Password);

                if (userSigninResult)
                {
                    _response.Success = true;
                    _response.Message = "Successfully login";
                    return _response;
                }

                _response.Success = false;
                _response.Message = "Email or password incorrect.";
                return _response;
            }
            catch (Exception ex)
            {

                _response.Success = false;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;

        }

        public async Task<ServiceResponse> CreateRoleAsync(string[] roles)
        {
            ServiceResponse _response = new();
            foreach (var role in roles)
            {
                if(!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role });
                    _response.Success = true;
                    _response.Message = "Created new role";
                    return _response;
                    
                }
            }
            return _response;
        }
    }
}
