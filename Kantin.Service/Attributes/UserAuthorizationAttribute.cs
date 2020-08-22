using Core.Helpers;
using Kantin.Data.Models;
using Kantin.Service.Models.Auth;
using Kantin.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Kantin.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UserAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly bool AllowAnonymous;
        private const string AuthorizationKey = "Authorization";
        private const string AuthorizationScheme = JwtBearerDefaults.AuthenticationScheme;
        private readonly string[] _privilegeNames;

        public UserAuthorizationAttribute(bool allowAnonymous = false)
        {
            AllowAnonymous = allowAnonymous;
            _privilegeNames = new string[] { };
        }

        public UserAuthorizationAttribute(string privilegeName)
        {
            AllowAnonymous = false;
            _privilegeNames = new string[] { privilegeName };
        }

        public UserAuthorizationAttribute(string[] privilegeNames)
        {
            AllowAnonymous = false;
            _privilegeNames = privilegeNames;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAuthorizationKey = context.HttpContext.Request.Headers.TryGetValue(AuthorizationKey, out var authorizationHeaderValue);

            if (!hasAuthorizationKey || string.IsNullOrEmpty(authorizationHeaderValue) || !HasAuthorizationScheme(authorizationHeaderValue))
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);

            var token = GetTokenFromAuthorizationHeaderValue(authorizationHeaderValue);
            var tokenService = context.HttpContext.RequestServices.GetService<ITokenAuthorizationService>();
            var session = tokenService.AuthorizeToken(token);

            if (session == null && !AllowAnonymous)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                SetHttpContextUser(session, context.HttpContext);

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

        private void SetHttpContextUser(Session session, HttpContext context)
        {
            if (session == null || context == null)
                return;

            var claim = new ClaimWrapper(session.Account);
            var role = session.Account.Privilege;

            var roles = new List<string>();

            if (role != null)
                roles.Add(role?.Name);

            context.User = new GenericPrincipal(claim, roles.ToArray());
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
