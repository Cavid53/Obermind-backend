using Service.Common;
using System.Threading.Tasks;

namespace Service.Account
{
    public interface IAccountService
    {
        Task<ServiceResponse> ResgisterAsync(UserSignUpResource userSignUpResource);
        Task<ServiceResponse> LoginAsync(UserSignInResource userSignInResource);
        Task<ServiceResponse> CreateRoleAsync(string[] roles);
    }
}
