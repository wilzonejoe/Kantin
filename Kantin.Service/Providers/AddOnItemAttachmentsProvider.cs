using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class AddOnItemAttachmentsProvider : GenericProvider<AddOnItemAttachment, KantinEntities>
    {
        public AddOnItemAttachmentsProvider(KantinEntities context) : base(context) { }
        public AddOnItemAttachmentsProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<AddOnItemAttachment> Get(Guid id)
        {
            var menuItem = await Context.AddOnItemAttachments
                .Include(m => m.AddOnItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                HandleItemNotFound(id);

            return menuItem;
        }

        protected override Task BeforeDelete(AddOnItemAttachment entity)
        {
            DeleteFileFromStorage(entity.Id);
            return base.BeforeDelete(entity);
        }

        private void DeleteFileFromStorage(Guid menuItemAttachmentId)
        {

        }
    }
}
