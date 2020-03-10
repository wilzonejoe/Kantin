using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models.Tag;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Service.Providers
{
    public class TagGroupProvider : GenericProvider<TagGroup, KantinEntities>
    {
        public TagGroupProvider(KantinEntities context) : base(context)
        {
        }

        public override async Task<TagGroup> Get(Guid id)
        {
            var item = await Context.TagGroups
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
                HandleItemNotFound(id);

            return item;
        }

        public override async Task<TagGroup> Update(Guid id, TagGroup entity)
        {
            ValidateEntity(entity);
            var newEntry = await Create(entity);
            await Delete(id);
            return newEntry;
        }
        
        public override async Task<bool> Delete(Guid id)
        {
            var item = await Get(id);
            var result = Context.Remove(item);
            await Context.SaveChangesAsync();
            return result != null;
        }
    }
}
