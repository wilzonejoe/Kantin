using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models.Tag;
using Kantin.Service.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Service.Providers
{
    public class TagGroupProvider : GenericProvider<TagGroup, KantinEntities>
    {
        public AccountIdentity AccountIdentity { get; private set; }
        public TagGroupProvider(KantinEntities context) : base(context)
        {
        }

        public TagGroupProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context)
        {
            AccountIdentity = accountIdentity;
        }

        public override async Task<TagGroup> Get(Guid id)
        {
            var item = await Context.TagGroups
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
                HandleItemNotFound(id);

            return item;
        }

        public override Task<TagGroup> Create(TagGroup entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            return base.Create(entity);
        }

        public override async Task<TagGroup> Update(Guid id, TagGroup entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            ValidateEntity(entity);
            var deleteItem = await GetById(id);
            if (deleteItem == null)
            {
                HandleItemNotFound(id);
                return null;
            }
            var newEntry = await Create(entity);
            Context.Remove(deleteItem);
            await Context.SaveChangesAsync();
            return newEntry;
        }

        private async Task<TagGroup> GetById(Guid id)
        {
            var item = await Context.TagGroups
                .FirstOrDefaultAsync(m => m.Id == id);

            return item;
        }
    }
}
