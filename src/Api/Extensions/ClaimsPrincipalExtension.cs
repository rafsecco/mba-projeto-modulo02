using System.Security.Claims;

namespace Api.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (claim is null)
            {
                throw new Exception("User ID not found in claims");
            }
            return Guid.Parse(claim.Value);
        }
    }
}