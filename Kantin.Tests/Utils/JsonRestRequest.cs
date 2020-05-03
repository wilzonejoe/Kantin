using RestSharp;
using RestSharp.Serialization.Json;

namespace Kantin.Tests.Utils
{
    public class JsonRestRequest : RestRequest, IRestRequest
    {
        private const string ContentType = "application/json";

        public JsonRestRequest(Method method, string resource) : base(resource, method)
        {
            RequestFormat = DataFormat.Json;
            AddHeader("Content-Type", ContentType);
            JsonSerializer = new JsonSerializer();
        }

        public JsonRestRequest(Method method, string resource, object body): this(method, resource)
        {
            AddJsonBody(body);
        }
    }
}