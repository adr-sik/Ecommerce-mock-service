using Client.Components;
using Client.Services;
using Microsoft.Extensions.FileProviders;
using MudBlazor.Services;
using Shared.Models.DTOs;
using Shared.Models.DTOs.ProductTypesDTOs;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMudServices();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            jsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            builder.Services.AddSingleton(jsonSerializerOptions);

            var apiUrl = builder.Configuration["ApiUrl"];

            AddApiService<ProductService<ProductDTO>>(builder, apiUrl);
            AddApiService<ProductService<LaptopDTO>>(builder, apiUrl);
            AddApiService<ProductService<PhoneDTO>>(builder, apiUrl);
            AddApiService<ProductService<HeadphonesDTO>>(builder, apiUrl);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.UseStatusCodePagesWithRedirects("/notfound");

            app.Run();
        }

        // Service registraction helper
        private static void AddApiService<TService>(WebApplicationBuilder builder, string apiUrl) where TService : class
        {
            Console.WriteLine($"Registering {typeof(TService).Name} with base URL {apiUrl}");
            builder.Services.AddHttpClient<TService>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
            });
        }
    }
}
