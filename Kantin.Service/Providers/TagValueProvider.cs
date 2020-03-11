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
    public class TagValueProvider : GenericProvider<TagValue, KantinEntities>
    {
        public AccountIdentity AccountIdentity { get; private set; }
        public TagValueProvider(KantinEntities context) : base(context)
        {
        }

        public TagValueProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context)
        {
            AccountIdentity = accountIdentity;
        }

        public override async Task<TagValue> Get(Guid id)
        {
            var item = await Context.TagValues
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
                HandleItemNotFound(id);

            return item;
        }

        public override async Task<TagValue> Create(TagValue entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            ValidateEntity(entity);
            entity.Id = Guid.NewGuid();
            bool checkItemExisted = await IsItemExisted(entity);
            bool checkValidTagGroup = await IsTagGroupIdValid(entity.TagGroupId);
            if (!checkItemExisted)
            {
                throw new ItemNotFoundException("Itemtype id: " + entity.ItemId + "not found in Itemtype: " + entity.ItemType);
            }

            if (!checkValidTagGroup)
            {
                throw new ItemNotFoundException("TagGroup id: " + entity.ItemId + " not found");
            }
            var result = await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }

        public override async Task<TagValue> Update(Guid id, TagValue entity)
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

        private async Task<bool> IsItemExisted(TagValue entity)
        {
            try
            {
                if (entity.ItemType == ItemType.MenuItem)
                {
                    var provider = new MenuItemsProvider(Context);
                    var itemEntity = await provider.Get(entity.ItemId);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> IsTagGroupIdValid(Guid id)
        {
            try
            {
                var provider = new TagGroupProvider(Context);
                var itemEntity = await provider.Get(id);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private async Task<TagValue> GetById(Guid id)
        {
            var item = await Context.TagValues
                .FirstOrDefaultAsync(m => m.Id == id);

            return item;
        }
    }
}
