using Core.Models.Auth;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Helpers;

namespace Kantin.Service.Services
{
    public interface ITokenAuthorizationService
    {
        public Session AuthorizeToken(string token);
        public Task SaveToken(string token, Guid accountId);
    }

    public class TokenAuthorizationService : ITokenAuthorizationService
    {
        public KantinEntities Context { get; private set; }
        public TokenAuthorizationService(KantinEntities context) { Context = context; }

        public Session AuthorizeToken(string token)
        {
            var tokenClaims = JWTHelper.Instance.GetTokenClaims(token);

            if (tokenClaims == null)
                return null;

            var accountClaim = tokenClaims.FirstOrDefault(tc => tc.Type == nameof(AccountIdentity.AccountId));
            var accountId = accountClaim.Value;

            if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var accountIdGuid))
                return null;

            return Context.Sessions
                .Include(s => s.Account)
                .ThenInclude(a => a.Privilege)
                .FirstOrDefault(s => s.AccountId == accountIdGuid);
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
