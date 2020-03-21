using Core.Exceptions;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Models.Auth;
using Kantin.Service.Providers;
using Kantin.Service.Services;
using Kantin.Tests.Utils;
using Kantin.Tests.Validator;
using NUnit.Framework;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Kantin.Tests.Case.Steps
{
    [Binding]
    public sealed class RegisterSteps
    {
        private readonly ScenarioContext Context;
        private readonly string LoginResultContext = "LoginResultContext";

        public RegisterSteps(ScenarioContext injectedContext)
        {
            Context = injectedContext;
        }

        [Given(@"I registered with (.*), (.*) and (.*)")]
        public void GivenIRegisteredWithAnd(string fullname, string username, string password)
        {
            Register(fullname, username, password);
        }

        [When(@"I registered with (.*), (.*) and (.*)")]
        public void WhenIRegisteredWithAnd(string fullname, string username, string password)
        {
            Register(fullname, username, password);
        }

        [Then(@"I can login successfully with (.*) and (.*)")]
        public void ThenICanLoginSuccessfullyWithAnd(string username, string password)
        {
            var currentTimestamp = ContextScout.GetCurrentTimestamp(Context);

            var login = new Login
            {
                Username = $"{username}{currentTimestamp}",
                Password = password
            };

            var dbContext = ContextScout.GetDBContextContainer(Context);

            var tokenService = new TokenAuthorizationService(dbContext);
            var provider = new AccountProvider(dbContext);
            var loginResult = provider.Login(tokenService, login).GetAwaiter().GetResult();

            LoginResultValidator.ValidateSuccessLoginResult(dbContext, loginResult);
        }

        [Then(@"LoginResult should be successful")]
        public void ThenLoginResultShouldBeSuccessful()
        {
            var dbContext = ContextScout.GetDBContextContainer(Context);

            if (Context[LoginResultContext] is LoginResult loginResult)
                LoginResultValidator.ValidateSuccessLoginResult(dbContext, loginResult);
            else
                Assert.Fail("Login result should valid");
        }

        [Then(@"LoginResult should be conflicted")]
        public void ThenLoginResultShouldBeConflicted()
        {
            Assert.IsTrue(Context[LoginResultContext] is ConflictException, "Login result should throw conflict exception");
        }


        [Then(@"Database contains account with (.*) and (.*)")]
        public void ThenDatabaseContainsAccountWithAnd(string fullname, string username)
        {
            var dbContext = ContextScout.GetDBContextContainer(Context);
            var currentTimestamp = ContextScout.GetCurrentTimestamp(Context);
            var registeredUsername = $"{username}{currentTimestamp}";
            var createdAccount = dbContext.Accounts.FirstOrDefault(o => o.Fullname == fullname && o.Username == registeredUsername);

            Assert.IsTrue(createdAccount != null, $"{nameof(Account)} with {nameof(Account.Username)} {registeredUsername} and {nameof(Account.Fullname)} {fullname} should have been created");
            Assert.IsTrue(createdAccount.OrganisationId == null, $"{nameof(Account)}.{nameof(Account.OrganisationId)} should be null");
        }

        private void Register(string fullname, string username, string password)
        {
            try
            {
                var currentTimestamp = ContextScout.GetCurrentTimestamp(Context);

                var register = new Register
                {
                    Fullname = fullname,
                    Username = $"{username}{currentTimestamp}",
                    Password = password,
                };

                var dbContext = ContextScout.GetDBContextContainer(Context);

                var tokenService = new TokenAuthorizationService(dbContext);
                var provider = new AccountProvider(dbContext);
                var loginResult = provider.Register(tokenService, register).GetAwaiter().GetResult();
                Context[LoginResultContext] = loginResult;
            }
            catch (ConflictException e)
            {
                Context[LoginResultContext] = e;
            }
            catch (BadRequestException e)
            {
                Context[LoginResultContext] = e;
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}
