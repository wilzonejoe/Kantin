using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Exceptions.Models;
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

        public override async Task<TagGroup> Create(TagGroup entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            ValidateEntity(entity);
            var item = await GetByTitle(entity.Title);
            if (item != null)
            {
                throw new ConflictException(new List<PropertyErrorResult>()
                {
                    new PropertyErrorResult
                    {
                        FieldErrors = "Title",
                        FieldName = $"The {nameof(TagGroup.Title)} with value {entity.Title} is already existed in the database"
                    }
                });
            }
            return await base.Create(entity);
        }

        public override Task<TagGroup> Update(Guid id, TagGroup entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            return base.Update(id, entity);
        }

        private async Task<TagGroup> GetById(Guid id)
        {
            var item = await Context.TagGroups
                .FirstOrDefaultAsync(m => m.Id == id);

            return item;
        }

        private async Task<TagGroup> GetByTitle(string Title)
        {
            var item = await Context.TagGroups
                .FirstOrDefaultAsync(m => m.Title == Title);

            return item;
        }
    }
}
