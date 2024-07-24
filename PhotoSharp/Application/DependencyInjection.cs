using Application.Images;
using Application.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options => options
            .UseNpgsql(connectionString));

        services.BuildServiceProvider().GetService<AppDbContext>()?.Database.Migrate();

        services.AddScoped<IImageService, ImageService>();

        services.Configure<SettingsOptions>(configuration.GetSection("Settings"));
        return services;
    }
}
