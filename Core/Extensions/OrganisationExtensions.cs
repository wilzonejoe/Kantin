using Core.Exceptions;
using Core.Models.Interfaces;
using System;

namespace Core.Extensions
{
    public static class OrganisationExtensions
    {
        public static void SetOrganisationId(this IOrganisationModel model, Guid? organisationId)
        {
            if (organisationId.HasValue)
                model.OrganisationId = organisationId.Value;
            else
                throw new ForbiddenException($"{nameof(IOrganisationModel.OrganisationId)} is not present");
        }
    }
}
