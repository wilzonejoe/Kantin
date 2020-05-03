using Kantin.Tests.Case;
using RestSharp;
using RestSharp.Serialization.Json;
using TechTalk.SpecFlow;

namespace Kantin.Tests.Utils
{
    public class RestSharpEndPoints
    {
        public static IRestResponse<T> ExecuteJsonRequest<T>(ScenarioContext context, IRestRequest request) where T : new()
        {
            var client = new RestClient(Constant.Url);
            client.AddHandler("application/json", () => new JsonDeserializer());

            if(context.ContainsKey(ContextContants.AccessTokenContext) &&
                context[ContextContants.AccessTokenContext] is string token && 
                !string.IsNullOrEmpty(token))
                request.AddHeader("Authorization", "Bearer " + token);

            var response = client.Execute<T>(request);
            context[ContextContants.ResponseContext] = response;
            return response;
        }
    }
}