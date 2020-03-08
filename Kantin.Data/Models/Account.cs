using Core.Models.Auth;
using System.Collections.Generic;

namespace Kantin.Data.Models
{
    public class Account : BaseAccount
    {
        public Organisation Organisation { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
