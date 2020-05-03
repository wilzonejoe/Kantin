using Core.Exceptions;
using Core.Model;
using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class StaffMemberProvider : GenericProvider<Account, KantinEntities>
    {
        private Guid OrganisationId
        {
            get
            {
                var organisationId = AccountIdentity?.OrganisationId;
                if (organisationId == null || organisationId == Guid.Empty)
                    throw new ForbiddenException("Organisation id is required");

                return organisationId.GetValueOrDefault();
            }
        }

        public StaffMemberProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<IEnumerable<Account>> GetAll(Query query)
        {
            return await Task.Run(() => Context.Accounts.Where(a => a.OrganisationId == OrganisationId));
        }

        public override async Task<Account> Get(Guid id)
        {
            var staffMember = await Context.Accounts
                .Include(a => a.Organisation)
                .Include(a => a.Privilege)
                .FirstOrDefaultAsync(a => a.Id == id && a.OrganisationId == OrganisationId);

            if (staffMember == null)
                HandleItemNotFound(id);

            var sessions = await Context.Sessions
                            .Where(s => s.AccountId == id)
                            .OrderBy(s => s.CreatedDateUTC)
                            .Take(5)
                            .ToListAsync();

            staffMember.Sessions = sessions;
            return staffMember;
        }

        protected override Task BeforeCreate(Account entity)
        {
            entity.OrganisationId = OrganisationId;
            return base.BeforeCreate(entity);
        }

        protected override Task BeforeUpdate(Account entity)
        {
            entity.OrganisationId = OrganisationId;
            return base.BeforeUpdate(entity);
        }
    }
}
