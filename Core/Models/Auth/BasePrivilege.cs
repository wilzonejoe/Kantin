using Core.Models.Abstracts;
using Core.Models.Interfaces;
using System;

namespace Core.Models.Auth
{
    public class BasePrivilege : BaseEntity, IOrganisationModel
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
