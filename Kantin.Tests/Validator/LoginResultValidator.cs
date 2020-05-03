using Kantin.Data;
using Kantin.Service.Models.Auth;
using Kantin.Tests.Models;
using NUnit.Framework;
using RestSharp;
using System.Linq;

namespace Kantin.Tests.Validator
{
    public static class LoginResultValidator
    {
        public static void ValidateSuccessLoginResult(KantinEntities context, IRestResponse<LoginResult> response)
        {
            var result = new ValidateResult();
            var loginResult = response.Data;

            if (!loginResult.Success)
            {
                result.Success = false;
                result.AppendMessage($"{nameof(LoginResult)}.{nameof(LoginResult.Success)} should be true to use this validator");
            }
            else if (string.IsNullOrEmpty(loginResult.Token))
            {
                result.Success = false;
                result.AppendMessage($"{nameof(LoginResult)}.{nameof(LoginResult.Token)} should not be null or empty");
            }
            else
            {
                var hasSession = context.Sessions.Any(s => s.Token == loginResult.Token);
                result.Success = hasSession;

                if (!hasSession)
                    result.AppendMessage($"{nameof(LoginResult)}.{nameof(LoginResult.Token)} should have a related session");
            }

            Assert.IsTrue(result.Success, result.Message);
        }

        public static void ValidateFailedLoginResult(IRestResponse<LoginResult> response)
        {
            var result = new ValidateResult();
            var loginResult = response.Data;

            if (loginResult.Success)
            {
                result.Success = false;
                result.AppendMessage($"{nameof(LoginResult)}.{nameof(LoginResult.Success)} should be false to use this validator");
            }

            if (!string.IsNullOrEmpty(loginResult.Token))
            {
                result.Success = false;
                result.AppendMessage($"{nameof(LoginResult)}.{nameof(LoginResult.Token)} should be empty");
            }

            Assert.IsTrue(result.Success, result.Message);
        }
    }
}
