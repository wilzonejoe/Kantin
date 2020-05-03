using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Kantin.Tests.Case.Steps
{
    [Binding]
    public sealed class CommonSteps
    {
        private readonly ScenarioContext Context;
        public CommonSteps(ScenarioContext injectedContext)
        {
            Context = injectedContext;
        }

        [Then(@"response code should be (.*)")]
        public void ThenResponseCodeShouldBe(string statusCode)
        {
            if (Context[ContextContants.ResponseContext] is IRestResponse response)
                Assert.IsTrue(((int)response.StatusCode).ToString() == statusCode, $"Response should throw {statusCode} but it threw {response.StatusCode}");
        }
    }
}
