using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VoiceTranscriptionApi.Attributes;

/// <summary>
/// Authorizes requests based on a token in the configuration.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TokenAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "Authorization";

    /// <summary>
    /// Called to authorize a request.
    /// </summary>
    /// <param name="context">The authorization filter context.</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        if (configuration is null)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return;
        }

        var apiKey = configuration["ApiSettings:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!apiKey.Equals(potentialApiKey))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
