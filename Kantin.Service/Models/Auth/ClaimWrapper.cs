using System.Security.Principal;
using Kantin.Data.Models;

namespace Kantin.Service.Models.Auth
{
    public class ClaimWrapper: IIdentity
    {
        public readonly Account Account;
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated => Account != null;
        public string Name => Account?.Fullname;

        public ClaimWrapper(Account account)
        {
            Account = account;
            AuthenticationType = "Password";
        }
    }
}
