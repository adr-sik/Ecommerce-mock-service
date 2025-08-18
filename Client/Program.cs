using Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddMudServices();

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            jsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            builder.Services.AddSingleton(jsonSerializerOptions);

            var apiUrl = builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress;
           
            AddApiService<ProductService>(builder, apiUrl);

            await builder.Build().RunAsync();
        }

        private static void AddApiService<TService>(WebAssemblyHostBuilder builder, string apiUrl) where TService : class
        {
            Console.WriteLine($"Registering {typeof(TService).Name} with base URL {apiUrl}");
            builder.Services.AddHttpClient<TService>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
            });
        }
    }
}
