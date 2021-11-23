using System;
using System.Net.Http;
using System.Threading.Tasks;
using AzureStaticWebApps.Blazor.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace TinyBacklog.Web.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var baseAddress = builder.HostEnvironment.BaseAddress;

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
            builder.Services.AddStaticWebAppsAuthentication();

            await builder.Build().RunAsync();
        }
    }
}