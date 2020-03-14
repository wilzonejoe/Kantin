using System;
using System.Collections.Generic;
using System.Linq;
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

        public override async Task<TagValue> Create(TagValue entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            ValidateEntity(entity);
            entity.Id = Guid.NewGuid();
            CheckExistingTagValue(entity);

            bool checkItemExisted = await IsItemExisted(entity);
            bool checkValidTagGroup = await IsTagGroupIdValid(entity.TagGroupId);
            if (!checkItemExisted)
                throw new ItemNotFoundException("Itemtype id: " + entity.ItemId + "not found in Itemtype: " + entity.ItemType);

            if (!checkValidTagGroup)
                throw new ItemNotFoundException("TagGroup id: " + entity.ItemId + " not found");
            
            var result = await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }

        public override Task<TagValue> Update(Guid id, TagValue entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            return base.Update(id, entity);
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

        private void CheckExistingTagValue(TagValue entity)
        {
            var titleExisted = false;
            var itemIdExisted = false;
            var itemTypeExisted = false;

            var itemList = Context.TagValues.Where(m => m.Title == entity.Title);

            if (itemList.Any())
            {
                titleExisted = true;
                itemIdExisted = itemList.Any(item => item.ItemId == entity.ItemId);
                itemTypeExisted = itemList.Any(item => item.ItemType == entity.ItemType);
            }

            if (!titleExisted || !itemIdExisted || !itemTypeExisted)
                return;

            throw new ConflictException(new List<PropertyErrorResult>()
            {
                new PropertyErrorResult
                {
                    FieldName = $"Tag Value with {nameof(TagValue.Title)} with value {entity.Title}, {nameof(TagValue.ItemId)} with value {entity.ItemId}, and {nameof(TagValue.ItemType)} with value {entity.ItemType} had already been existed in the database"
                }
            });
        }
    }
}
