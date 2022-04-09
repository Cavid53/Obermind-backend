using Domain.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Service.Account
{


    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                UserId = string.IsNullOrEmpty(userId) ? null : userId;
                IsAuthenticated = userId != null;
            }
        }

        public string UserId { get; }
        public bool IsAuthenticated { get; }
    }
}
