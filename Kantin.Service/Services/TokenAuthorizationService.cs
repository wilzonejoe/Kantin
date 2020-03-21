using Core.Models.Auth;
using Kantin.Data;
using Kantin.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kantin.Service.Services
{
    public interface ITokenAuthorizationService
    {
        public bool AuthorizeToken(string token);
        public Task SaveToken(string token, Guid accountId);
    }

    public class TokenAuthorizationService : ITokenAuthorizationService
    {
        public KantinEntities Context { get; private set; }
        public TokenAuthorizationService(KantinEntities context) { Context = context; }

        public bool AuthorizeToken(string token)
        {
            return Context.Sessions
                .Any(s => s.Token == token);
        }

        public async Task SaveToken(string token, Guid accountId)
        {
            var session = new Session
            {
                AccountId = accountId,
                Token = token
            };

            await Context.AddAsync(session);
            Context.SaveChanges();
        }
    }
}
