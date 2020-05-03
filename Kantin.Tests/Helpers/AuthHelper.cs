using Kantin.Service.Models.Auth;
using Kantin.Tests.Case;
using Kantin.Tests.Utils;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;

namespace Kantin.Tests.Helpers
{
    public class AuthHelper
    {
        private readonly ScenarioContext _context;

        public AuthHelper(ScenarioContext context)
        {
            _context = context;
        }

        public IRestResponse<LoginResult> Register(string fullname, string username, string password)
        {
            var register = new Register
            {
                Fullname = fullname,
                Username = username,
                Password = password,
            };

            var request = new JsonRestRequest(Method.POST, ServiceConstant.Register, register);
            var result = RestSharpEndPoints.ExecuteJsonRequest<LoginResult>(_context, request);
            _context[ContextContants.LoginResultContext] = result;
            SetAccessTokenContext(result);
            return result;
        }

        public IRestResponse<LoginResult> Login(string username, string password)
        {
            var login = new Login
            {
                Username = username,
                Password = password,
            };
            var request = new JsonRestRequest(Method.POST, ServiceConstant.Login, login);
            var result = RestSharpEndPoints.ExecuteJsonRequest<LoginResult>(_context, request);
            _context[ContextContants.LoginResultContext] = result;
            SetAccessTokenContext(result);
            return result;
        }

        private void SetAccessTokenContext(IRestResponse<LoginResult> result)
        {
            if (result.StatusCode == HttpStatusCode.OK)
                _context[ContextContants.AccessTokenContext] = result?.Data?.Token;
        }
    }
}
