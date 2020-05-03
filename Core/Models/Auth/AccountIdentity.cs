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
    }
}
