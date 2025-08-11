
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<EcommerceContext>(options =>
                options.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=ecom;Integrated Security=True;TrustServerCertificate=True"), ServiceLifetime.Scoped);

            builder.Services.AddControllers();

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    //policy.WithOrigins("https://localhost:7128") // your client app URL
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<EcommerceContext>();

                    context.Database.EnsureDeleted();
                    context.Database.Migrate();

                    SeedData.Initialize(context);
                }
            }

            app.UseHttpsRedirection();

            app.UseCors();  // <-- Use CORS middleware here

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

    }
}
