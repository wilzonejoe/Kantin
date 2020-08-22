using Core.Models.Auth;
using Newtonsoft.Json;

namespace Kantin.Data.Models
{
    public class Privilege : BasePrivilege
    {
        public string Name { get; set; }
        public bool CanAccessMenu { get; set; }
        public bool CanAccessOrder { get; set; }
        public bool CanAccessStaffMember { get; set; }
        public bool CanAccessSettings { get; set; }

        [JsonIgnore]
        public Account Account { get; set; }

        [JsonIgnore]
        public Organisation Organisation { get; set; }
    }
}
