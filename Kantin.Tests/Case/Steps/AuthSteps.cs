using Kantin.Data.Models;
using Kantin.Service.Models.Auth;
using Kantin.Tests.Helpers;
using Kantin.Tests.Utils;
using Kantin.Tests.Validator;
using NUnit.Framework;
using RestSharp;
using System.Linq;
using TechTalk.SpecFlow;

namespace Kantin.Tests.Case.Steps
{
    [Binding]
    public sealed class AuthSteps
    {
        private readonly ScenarioContext Context;
        private readonly AuthHelper _authHelper;

        public AuthSteps(ScenarioContext injectedContext)
        {
            Context = injectedContext;
            _authHelper = new AuthHelper(Context);
        }

        [Given(@"I registered with (.*), (.*) and (.*)")]
        public void GivenIRegisteredWithAnd(string fullname, string username, string password)
        {
            _authHelper.Register(fullname, StepsHelper.GetTimestampedName(Context, username), password);
        }

        [When(@"I registered with (.*), (.*) and (.*)")]
        public void WhenIRegisteredWithAnd(string fullname, string username, string password)
        {
            _authHelper.Register(fullname, StepsHelper.GetTimestampedName(Context, username), password);
        }

        [Then(@"I can login successfully with (.*) and (.*)")]
        public void ThenICanLoginSuccessfullyWithAnd(string username, string password)
        {
            var dbContext = ContextScout.GetDBContextContainer(Context);

            var response = _authHelper.Login(StepsHelper.GetTimestampedName(Context, username), password);
            LoginResultValidator.ValidateSuccessLoginResult(dbContext, response);
        }

        [Then(@"LoginResult should be successful")]
        public void ThenLoginResultShouldBeSuccessful()
        {
            var dbContext = ContextScout.GetDBContextContainer(Context);

            if (Context[ContextContants.LoginResultContext] is IRestResponse<LoginResult> response)
                LoginResultValidator.ValidateSuccessLoginResult(dbContext, response);
            else
                Assert.Fail("Login result should valid");
        }

        [Then(@"Database contains account with (.*) and (.*)")]
        public void ThenDatabaseContainsAccountWithAnd(string fullname, string username)
        {
            var dbContext = ContextScout.GetDBContextContainer(Context);
            var currentTimestamp = ContextScout.GetCurrentTimestamp(Context);
            var registeredUsername = StepsHelper.GetTimestampedName(Context, username);
            var createdAccount = dbContext.Accounts.FirstOrDefault(o => o.Fullname == fullname && o.Username == registeredUsername);

            Assert.IsTrue(createdAccount != null, $"{nameof(Account)} with {nameof(Account.Username)} {registeredUsername} and {nameof(Account.Fullname)} {fullname} should have been created");
            Assert.IsTrue(createdAccount.OrganisationId == null, $"{nameof(Account)}.{nameof(Account.OrganisationId)} should be null");
        }
    }
}
