using Kantin.Data.Models;
using System;
using System.Collections.Generic;

namespace Kantin.Models.Request
{
    public class EditableMenuRequest : Menu
    {
        public List<Guid> MenuItemIds { get; set; }

        public EditableMenuRequest()
        {
            MenuItemIds = new List<Guid>();
        }
    }
}
