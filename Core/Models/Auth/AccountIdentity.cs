using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Core.Models.Auth
{
    public class AccountIdentity
    {
        public Guid? OrganisationId { get; set; }
        public Guid? AccountId { get; set; }

        public AccountIdentity(IEnumerable<Claim> claims)
        {
            Init(claims);
        }

        private void Init(IEnumerable<Claim> claims)
        {
            var organisationIdString = JWTHelper.Instance.GetValueFromClaims(claims, nameof(OrganisationId));
            var isOrganisationIdParsed = Guid.TryParse(organisationIdString, out var organisationId);

            if (isOrganisationIdParsed)
                OrganisationId = organisationId;

            var accountIdString = JWTHelper.Instance.GetValueFromClaims(claims, nameof(AccountId));
            var isAccountIdParsed = Guid.TryParse(accountIdString, out var accountId);

            if (isAccountIdParsed)
                AccountId = accountId;
        }
    }
}
