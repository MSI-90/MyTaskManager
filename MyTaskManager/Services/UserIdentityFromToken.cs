using MyTaskManager.Services.Interfaces;
using System.Security.Claims;

namespace MyTaskManager.Services
{
    public class UserIdentityFromToken : IGetUserIdentity
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public int UserClaimsCount { get; private set; }
        public UserIdentityFromToken(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;
        public IEnumerable<string> GetClaims()
        {
            List<string> decodeClaims = new List<string>();

            var verifyedUser = _httpContextAccessor.HttpContext?.User?.Identity;
            var usersClaims = verifyedUser as ClaimsIdentity;

            if (usersClaims != null)
            {
                foreach (var userClaim in usersClaims.Claims)
                    decodeClaims.Add(userClaim.Value);
            }
            UserClaimsCount = decodeClaims.Count;

            return decodeClaims;
        }
    }
}
