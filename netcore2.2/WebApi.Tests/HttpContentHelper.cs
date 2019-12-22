using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace WebApi.Tests
{
    public static class HttpContentHelper
    {
        public static HttpContent GetJsonContent(object model) =>
            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
    }
}
