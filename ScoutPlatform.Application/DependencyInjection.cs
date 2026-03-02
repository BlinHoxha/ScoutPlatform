using Microsoft.Extensions.DependencyInjection;
using ScoutPlatform.Application.Players;
using ScoutPlatform.Application.Rankings;
using ScoutPlatform.Application.TeamProfiles;

namespace ScoutPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<PlayerService>();
        services.AddScoped<TeamProfileService>();
        services.AddScoped<RankingService>();

        return services;
    }
}
