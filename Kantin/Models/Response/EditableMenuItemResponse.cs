using Kantin.Data.Models;
using System.Collections.Generic;

namespace Kantin.Models.Response
{
    public class EditableMenuItemResponse : MenuItem
    {
        public List<AddOnItem> AddOnItems { get; set; }

        public EditableMenuItemResponse() : base()
        {
            AddOnItems = new List<AddOnItem>();
        }
    }
}
