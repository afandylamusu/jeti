using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Jet.Service.Manifest.UnitTest.Client
{
    [Collection("IdentityClient")]
    public class UnitTest1
    {
        private readonly IConfiguration _settings;
        private readonly ITestOutputHelper _testOutputHelper;
        private HttpClient _client;


        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _settings = Initialize.LoadSettings();
            _testOutputHelper = testOutputHelper;

        }
        //[Fact(DisplayName = "Test Manifest WebAPI Host")]
        public async Task Test_Host()
        {
            System.Threading.Thread.Sleep(5000);

            using (_client = new HttpClient())
            {
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                _client.DefaultRequestHeaders.Add("User-Agent", "XUnit JET");

                var expectedResponseStatusCode = System.Net.HttpStatusCode.OK;

                var url = _settings["ManifestWebApiServer"] + "/api/values";

                foreach (var o in _settings.AsEnumerable())
                {
                    _testOutputHelper.WriteLine(string.Format("{0}: {1}", o.Key, o.Value));
                }

                var response = await _client.GetAsync(url);
                Assert.IsType<HttpResponseMessage>(response);
                Assert.Equal(expectedResponseStatusCode, response.StatusCode);
            }
        }

        [Fact(DisplayName = "Test Identity Server")]
        public async Task TestAuthToken()
        {
            var identityUrl = _settings.GetValue<string>("IdentityApiExternal");
            var manifestUrl = _settings.GetValue<string>("ManifestApiExternal");

            var url = DiscoveryClient.ParseUrl(identityUrl);
            var disco = await DiscoveryClient.GetAsync(url.authority);

            var tokenClient = new TokenClient(identityUrl + "/connect/token", "xunit", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(scope: "manifest");

            Assert.False(tokenResponse.IsError, "getting token failed");
            _testOutputHelper.WriteLine(tokenResponse.Json.ToString());

            using (var client = new HttpClient())
            {
                client.SetBearerToken(tokenResponse.AccessToken);
                var response = await client.GetAsync(manifestUrl + "/api/manifests");
                if (!response.IsSuccessStatusCode)
                {
                    _testOutputHelper.WriteLine(response.StatusCode.ToString());
                    Assert.False(true, response.Content.ToString());
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _testOutputHelper.WriteLine(JArray.Parse(content).ToString());
                }
            }
        }
    }
}
