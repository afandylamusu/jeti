using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Jet.Service.Manifest.UnitTest.Client.Fixtures
{
    public class IdentityServiceFixture : IDisposable
    {
        protected TimeSpan TestTimeout = TimeSpan.FromSeconds(60);
        private readonly IConfiguration _settings;

        public IdentityServiceFixture()
        {
            _settings = Initialize.LoadSettings();
            bool success = WaitForService(_settings.GetValue<string>("IdentityApiExternal")).Result;
            success = WaitForService(_settings.GetValue<string>("ManifestApiExternal")).Result;
        }

        public void Dispose()
        {
            
        }

        private async Task<bool> WaitForService(string TestUrl)
        {
            if (string.IsNullOrEmpty(TestUrl)) return false;

            using (var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(1) })
            {
                var startTime = DateTime.Now;
                while (DateTime.Now - startTime < TestTimeout)
                {
                    try
                    {
                        var response = await client.GetAsync(new Uri(TestUrl)).ConfigureAwait(false);
                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        // Ignore exceptions, just retry
                    }

                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }

            throw new Exception($"Startup failed, could not get '{TestUrl}' after trying for '{TestTimeout}'");
        }
    }
}
