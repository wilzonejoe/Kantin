using Kantin.Data.Models;
using Kantin.Tests.Helpers;
using Kantin.Tests.Utils;
using RestSharp;
using TechTalk.SpecFlow;

namespace Kantin.Tests.Case.Steps
{
    [Binding]
    public sealed class OrganisationSteps
    {
        private readonly ScenarioContext Context;
        public OrganisationSteps(ScenarioContext injectedContext)
        {
            Context = injectedContext;
        }

        [Given(@"I created an organisation with name (.*)")]
        public void GivenICreatedAnOrganisationWithName(string organisationName)
        {
            CreateOrganisation(organisationName);
        }

        [When(@"I created an organisation with name (.*)")]
        public void WhenICreatedAnOrganisationWithName(string organisationName)
        {
            CreateOrganisation(organisationName);
        }

        private void CreateOrganisation(string organisationName)
        {
            var organisation = new Organisation
            {
                Name = StepsHelper.GetTimestampedName(Context, organisationName)
            };

            var request = new JsonRestRequest(Method.POST, ServiceConstant.Organisation, organisation);
            var result = RestSharpEndPoints.ExecuteJsonRequest<Organisation>(Context, request);
            Context[ContextContants.OrganisationResultContext] = result;
        }
    }
}
