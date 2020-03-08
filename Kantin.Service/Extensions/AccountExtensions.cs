using Core.Models.Auth;
using Kantin.Data.Models;
using Kantin.Service.Models.Auth;
using System.Security.Claims;

namespace Kantin.Service.Extensions
{
    public static class AccountExtensions
    {
        public static JWTContainer ToJWTContainer(this Account account)
        {
            var claims = new[]
                {
                    new Claim(nameof(AccountIdentity.AccountId), account.Id.ToString()),
                    new Claim(nameof(AccountIdentity.OrganisationId), account.OrganisationId.ToString())
                };

            return new JWTContainer()
            {
                Claims = claims
            };
        }
    }
}
