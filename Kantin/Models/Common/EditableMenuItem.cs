using Kantin.Data.Models;
using System.Collections.Generic;

namespace Kantin.Models.Common
{
    public class EditableMenuItem : MenuItem
    {
        public List<AddOnItem> AddOnItems { get; set; }

        public EditableMenuItem() : base()
        {
            AddOnItems = new List<AddOnItem>();
        }
    }
}
