using Core.Helpers;
using Core.Models.Auth;
using Kantin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Kantin.Service.Services
{
    public class AccountIdentityService
    {
        public static AccountIdentity GenerateAccountIdentityFromClaims(KantinEntities context, IEnumerable<Claim> claims)
        {
            var accountIdentity = new AccountIdentity();

            var accountIdString = JWTHelper.Instance.GetValueFromClaims(claims, nameof(AccountIdentity.AccountId));
            var isAccountIdParsed = Guid.TryParse(accountIdString, out var accountId);

            if (isAccountIdParsed)
                accountIdentity.AccountId = accountId;

            var organisationIdString = JWTHelper.Instance.GetValueFromClaims(claims, nameof(AccountIdentity.OrganisationId));
            var isOrganisationIdParsed = Guid.TryParse(organisationIdString, out var organisationId);

            if (isOrganisationIdParsed && organisationId != Guid.Empty)
            {
                accountIdentity.OrganisationId = organisationId;
            }
            else if (accountId != null || accountId == Guid.Empty)
            {
                var account = context.Accounts.FirstOrDefault(a => a.Id == accountId);
                accountIdentity.OrganisationId = account?.OrganisationId;
            }

            return accountIdentity;
        }
    }
}
