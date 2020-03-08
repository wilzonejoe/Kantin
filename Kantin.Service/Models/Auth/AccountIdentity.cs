using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Kantin.Service.Models.Auth
{
    public class AccountIdentity
    {
        public Guid OrganisationId { get; set; }
        public Guid AccountId { get; set; }

        public AccountIdentity(IEnumerable<Claim> claims)
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
