using Kantin.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Kantin.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UserAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private const string AuthorizationKey = "Authorization";
        private const string AuthorizationScheme = JwtBearerDefaults.AuthenticationScheme;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAuthorizationKey = context.HttpContext.Request.Headers.TryGetValue(AuthorizationKey, out var authorizationHeaderValue);

            if (!hasAuthorizationKey || string.IsNullOrEmpty(authorizationHeaderValue) || !HasAuthorizationScheme(authorizationHeaderValue))
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);

            var token = GetTokenFromAuthorizationHeaderValue(authorizationHeaderValue);
            var tokenService = context.HttpContext.RequestServices.GetService<ITokenAuthorizationService>();
            var tokenAuthorized = tokenService.AuthorizeToken(token);

            if (!tokenAuthorized)
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
        }

        private bool HasAuthorizationScheme(string authorizationHeaderValue)
        {
            var token = GetTokenFromAuthorizationHeaderValue(authorizationHeaderValue);
            return !string.IsNullOrEmpty(token);
        }

        private string GetTokenFromAuthorizationHeaderValue(string authorizationHeaderValue)
        {
            if (string.IsNullOrEmpty(authorizationHeaderValue))
                return null;

            var tokenParts = authorizationHeaderValue.Trim().Split(' ');

            if (tokenParts.Count() < 2)
                return null;

            var authorizationScheme = tokenParts.FirstOrDefault();
            if (authorizationScheme == null || !authorizationScheme.Equals(AuthorizationScheme))
                return null;

            return tokenParts.LastOrDefault();
        }
    }
}
