using Client.Authorization;
using Client.Services;
using Client.Util;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Polly;
using Shared.Models.DTOs;
using Shared.Models.DTOs.ProductTypesDTOs;
using System;
using System.Net;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddMudServices();

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            jsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            builder.Services.AddSingleton(jsonSerializerOptions);

            var apiUrl = new Uri(builder.Configuration["ApiUrl"]!);

            builder.Services.AddScoped<ProductServiceResolver>();

            builder.Services.AddScoped<ProductService<ProductDTO>>();
            builder.Services.AddScoped<ProductService<LaptopDTO>>();
            builder.Services.AddScoped<ProductService<PhoneDTO>>();
            builder.Services.AddScoped<ProductService<HeadphonesDTO>>();

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<FilterStateService>();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<JwtAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());

            builder.Services.AddTransient<CookieForwardingHandler>();
            builder.Services.AddScoped<CartService>();

            //TODO : Optimize request handling 
            
            // General use client
            builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = apiUrl)
                .AddHttpMessageHandler<CookieForwardingHandler>()
                .AddPolicyHandler((serviceProvider, request) =>
                {
                    var policy = Policy.HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Unauthorized)
                        .RetryAsync(1, async (result, retryCount, context) =>
                        {
                            var authService = serviceProvider.GetRequiredService<AuthService>();
                            await authService.RefreshCookies();                           
                        });

                    return policy;
                });

            // Endpoint for refreshing authorization
            builder.Services.AddHttpClient("RefreshClient", client => client.BaseAddress = apiUrl)
                .AddHttpMessageHandler<CookieForwardingHandler>();


            await builder.Build().RunAsync();
        }
    }
}
