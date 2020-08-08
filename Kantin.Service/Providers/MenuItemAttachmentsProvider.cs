using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class MenuItemAttachmentsProvider : GenericProvider<MenuItemAttachment, KantinEntities>
    {
        public MenuItemAttachmentsProvider(KantinEntities context) : base(context) { }
        public MenuItemAttachmentsProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<MenuItemAttachment> Get(Guid id)
        {
            var menuItem = await Context.MenuItemAttachments
                .Include(m => m.MenuItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                HandleItemNotFound(id);

            return menuItem;
        }

        protected override Task BeforeDelete(MenuItemAttachment entity)
        {
            DeleteFileFromStorage(entity.Id);
            return base.BeforeDelete(entity);
        }

        private void DeleteFileFromStorage(Guid menuItemAttachmentId)
        {

        }
    }
}
