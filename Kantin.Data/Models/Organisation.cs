using Core.Models.Auth;
using System.Collections.Generic;

namespace Kantin.Data.Models
{
    public class Organisation : BaseOrganisation
    {
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<AddOnItem> AddOnItems { get; set; }
    }
}
