using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Exceptions.Models;
using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Service.Providers
{
    public class TagGroupProvider : GenericProvider<TagGroup, KantinEntities>
    {
        public TagGroupProvider(KantinEntities context) : base(context) { }

        public TagGroupProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        protected override async Task BeforeCreate(TagGroup entity)
        {
            await CheckIfTitleExisting(entity.Title);
            await base.BeforeCreate(entity);
        }

        protected override async Task BeforeUpdate(TagGroup entity)
        {
            await CheckIfTitleExisting(entity.Title);
            await base.BeforeUpdate(entity);
        }

        private async Task CheckIfTitleExisting(string title)
        {
            var item = await Context.TagGroups
                .FirstOrDefaultAsync(m => m.Title == title);

            if (item == null)
                return;

            throw new ConflictException(new List<PropertyErrorResult>()
                {
                    new PropertyErrorResult
                    {
                        FieldErrors = "Title",
                        FieldName = $"The {nameof(TagGroup.Title)} with value {title} is already existed in the database"
                    }
                });
        }
    }
}
