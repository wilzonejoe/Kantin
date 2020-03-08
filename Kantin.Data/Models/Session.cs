using Core.Models.Auth;

namespace Kantin.Data.Models
{
    public class Session : BaseSession
    {
        public Account Account { get; set; }
    }
}
