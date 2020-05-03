using Core.Models.Auth;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kantin.Data.Models
{
    public class Account : BaseAccount
    {
        [JsonIgnore]
        public Organisation Organisation { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Privilege Privilege { get; set; }

        [JsonIgnore]
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
