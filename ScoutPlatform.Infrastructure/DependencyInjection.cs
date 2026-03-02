using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScoutPlatform.Application.Common;
using ScoutPlatform.Domain.Scoring;
using ScoutPlatform.Infrastructure.Persistence;
using ScoutPlatform.Infrastructure.Scoring;

namespace ScoutPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' was not found.");

        services.AddDbContext<ScoutPlatformDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IPlayerRepository, EfPlayerRepository>();
        services.AddScoped<ITeamProfileRepository, EfTeamProfileRepository>();
        services.AddScoped<IMetricDefinitionRepository, EfMetricDefinitionRepository>();
        services.AddScoped<IScoringService, McdaScoringService>();

        return services;
    }
}
