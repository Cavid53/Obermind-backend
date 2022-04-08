using Service.Common;
using System.Threading.Tasks;

namespace Service.Account
{
    public interface IAccountService
    {
        Task<ServiceResponse<string>> ResgisterAsync(UserSignUpResource userSignUpResource);
        Task<ServiceResponse<string>> LoginAsync(UserSignInResource userSignInResource);
        Task<ServiceResponse<string>> CreateRoleAsync(string[] roles);
        Task<ServiceResponse<string>> AddUserToRoleAsync(string userEmail, string roleName);
    }
}
