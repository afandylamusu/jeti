using Microsoft.Extensions.Configuration;
using System;

namespace Jet.Service.Manifest.UnitTest
{
    public static class Initialize
    {
        public static IConfiguration LoadSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile($"settings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"settings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);

            builder.AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
