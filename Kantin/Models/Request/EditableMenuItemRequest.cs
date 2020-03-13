using Kantin.Data.Models;
using System;
using System.Collections.Generic;

namespace Kantin.Models.Request
{
    public class EditableMenuItemRequest : MenuItem
    {
        public List<Guid> AddOnItemIds { get; set; }

        public EditableMenuItemRequest() : base()
        {
            AddOnItemIds = new List<Guid>();
        }
    }
}
