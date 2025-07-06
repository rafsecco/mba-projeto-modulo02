using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Business.Extensions;

public class CustomAuthorization
{
	public static bool ValidarCalimsUsuario(HttpContext httpContext, string claimName, string claimValue)
	{
		if (httpContext.User.Identity == null) throw new InvalidOperationException();

		return httpContext.User.Identity.IsAuthenticated && httpContext.User.Claims.Any(c =>
			c.Type == claimName && c.Value.Contains(claimValue)
		);
	}
}

public class RequisitoClaimFilter : IAuthorizationFilter
{

	private readonly Claim _claim;

	public RequisitoClaimFilter(Claim claim)
	{
		_claim = claim;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		if (context.HttpContext.User.Identity == null) throw new InvalidOperationException();

		if (!context.HttpContext.User.Identity.IsAuthenticated)
		{
			if (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<Controller>() != null)
			{
				context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
				{
					area = "Identity",
					page = "/Account/Login",
					ReturnUrl = context.HttpContext.Request.Path.ToString()
				}));

			}
			context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
			return;
		}

		if (!CustomAuthorization.ValidarCalimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
		{
			context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
		}
	}
}

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
	public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
	{
		Arguments = [new Claim(claimName, claimValue)];
	}
}
