using Core.Helpers;
using Kantin.Data.Models;
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
        private readonly string[] _privilegeNames;

        public UserAuthorizationAttribute() { _privilegeNames = new string[] { }; }

        public UserAuthorizationAttribute(string privilegeName) { _privilegeNames = new string[] { privilegeName }; }

        public UserAuthorizationAttribute(string[] privilegeNames) { _privilegeNames = privilegeNames; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAuthorizationKey = context.HttpContext.Request.Headers.TryGetValue(AuthorizationKey, out var authorizationHeaderValue);

            if (!hasAuthorizationKey || string.IsNullOrEmpty(authorizationHeaderValue) || !HasAuthorizationScheme(authorizationHeaderValue))
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);

            var token = GetTokenFromAuthorizationHeaderValue(authorizationHeaderValue);
            var tokenService = context.HttpContext.RequestServices.GetService<ITokenAuthorizationService>();
            var session = tokenService.AuthorizeToken(token);

            if (session == null)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                if (session?.Account?.OrganisationId == null)
                    return;

                if (session?.Account?.Privilege == null)
                    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);

                if (_privilegeNames == null || !_privilegeNames.Any())
                    return;

                if (!CanAccessController(session))
                    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            }
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

        private bool CanAccessController(Session session)
        {
            var privilege = session?.Account?.Privilege;
            foreach (var privilegeNames in _privilegeNames)
            {
                var hasPrivilege = ReflectionsHelper.FindValueFromObject<bool, Privilege>(privilege, privilegeNames);

                if (!hasPrivilege)
                    return false;
            }

            return true;
        }
    }
}
