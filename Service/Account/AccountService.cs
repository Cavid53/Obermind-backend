using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;

        public AccountService(IMapper mapper, 
                              UserManager<AppUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;

        }
        public async Task<ServiceResponse<string>> ResgisterAsync(UserSignUpResource userSignUpResource)
        {
            ServiceResponse<string> _response = new();

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

        public async Task<ServiceResponse<string>> LoginAsync(UserSignInResource userSignInResource)
        {
            ServiceResponse<string> _response = new();

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
                    var roles = await _userManager.GetRolesAsync(user);
                    _response.Success = true;
                    _response.Message = "Successfully login";
                    _response.Data = GenerateJwt(user, roles);
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

        public async Task<ServiceResponse<string>> CreateRoleAsync(string[] roles)
        {
            ServiceResponse<string> _response = new();
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

        public async Task<ServiceResponse<string>> AddUserToRoleAsync(string userEmail, string roleName)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.UserName == userEmail);
                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    _response.Success = true;
                    _response.Message = "Successfully added";
                    return _response;
                }

                _response.Success = false;
                _response.Message = $"{roleName} - does not exist";
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

        private string GenerateJwt(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpirationInDays));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
