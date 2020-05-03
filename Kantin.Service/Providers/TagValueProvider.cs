using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Exceptions.Models;
using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Enumerable;
using Kantin.Data.Models;

namespace Kantin.Service.Providers
{
    public class TagValueProvider : GenericProvider<TagValue, KantinEntities>
    {
        public TagValueProvider(KantinEntities context) : base(context) { }

        public TagValueProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        protected override Task BeforeCreate(TagValue entity)
        {
            CheckExistingTagValue(entity);
            CheckIfTargetItemIsValid(entity);
            CheckTagGroupIdValid(entity.TagGroupId);
            return base.BeforeCreate(entity);
        }

        private void CheckIfTargetItemIsValid(TagValue entity)
        {
            var isTargetItemValid = false;

            switch (entity.ItemType)
            {
                case TagItemType.Menu:
                    isTargetItemValid = Context.Menus.Any(m => m.Id == entity.ItemId && 
                        m.OrganisationId == AccountIdentity.OrganisationId);
                    break;
                case TagItemType.MenuItem:
                    isTargetItemValid = Context.MenuItems.Any(m => m.Id == entity.ItemId && 
                        m.OrganisationId == AccountIdentity.OrganisationId);
                    break;
                case TagItemType.AddOnItem:
                    isTargetItemValid = Context.AddOnItems.Any(m => m.Id == entity.ItemId &&
                        m.OrganisationId == AccountIdentity.OrganisationId);
                    break;
                default:
                    isTargetItemValid = false;
                    break;
            }


            if (isTargetItemValid)
                return;

            var propertyValidationErrors = new List<PropertyErrorResult>
            {
                new PropertyErrorResult
                {
                    FieldName = nameof(TagValue.ItemType),
                    FieldErrors = $"{nameof(TagValue.ItemType)} ({entity.ItemType}) is not valid"
                }
            };

            throw new BadRequestException(propertyValidationErrors);
        }

        private void CheckTagGroupIdValid(Guid tagGroupId)
        {
            var tagGroupExisted = Context.TagGroups.Any(tg => tg.Id == tagGroupId);

            if (tagGroupExisted)
                return;

            var propertyValidationErrors = new List<PropertyErrorResult>
            {
                new PropertyErrorResult
                {
                    FieldName = nameof(TagValue.TagGroupId),
                    FieldErrors = $"{nameof(TagValue.TagGroupId)} ({tagGroupId}) is not found"
                }
            };

            throw new BadRequestException(propertyValidationErrors);
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
