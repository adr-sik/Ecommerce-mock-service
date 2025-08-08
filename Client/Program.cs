using Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var apiUrl = builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress;
           
            AddApiService<OrderService>(builder, apiUrl);
            AddApiService<ProductService>(builder, apiUrl);
            AddApiService<UserService>(builder, apiUrl);
            AddApiService<DesignTimeProductService>(builder, apiUrl);

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
