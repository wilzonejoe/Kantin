﻿using Kantin.Data.Models;
using System.Collections.Generic;

namespace Kantin.Models.Response
{
    public class EditableMenuResponse : Menu
    {
        public List<MenuItem> MenuItems { get; set; }

        public EditableMenuResponse() : base()
        {
            MenuItems = new List<MenuItem>();
        }
    }
}
