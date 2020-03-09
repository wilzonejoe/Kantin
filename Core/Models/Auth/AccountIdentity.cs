using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Core.Models.Auth
{
    public class UserIdentity
    {
        public Guid OrganisationId { get; set; }
        public Guid AccountId { get; set; }

        public UserIdentity(IEnumerable<Claim> claims)
        {
            Init(claims);
        }

        private void Init(IEnumerable<Claim> claims)
        {
            var organisationIdString = JWTHelper.Instance.GetValueFromClaims(claims, nameof(OrganisationId));
            OrganisationId = Guid.Parse(organisationIdString);

            var accountId = JWTHelper.Instance.GetValueFromClaims(claims, nameof(AccountId));
            AccountId = Guid.Parse(accountId);
        }
    }
}
