using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace VoiceTranscriptionApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TokenAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            if (configuration == null)
            {
                context.Result = new UnauthorizedObjectResult("خطای سرور: سرویس پیکربندی در دسترس نیست.");
                return;
            }

            var token = configuration.GetValue<string>("ApiSettings:Token");

            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                context.Result = new UnauthorizedObjectResult("توکن احراز هویت در هدر درخواست وجود ندارد.");
                return;
            }

            var providedToken = tokenHeader.ToString();
            if (providedToken != token)
            {
                context.Result = new UnauthorizedObjectResult("توکن احراز هویت نامعتبر است.");
            }
        }
    }
} 