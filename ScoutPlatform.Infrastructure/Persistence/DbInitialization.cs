using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ScoutPlatform.Infrastructure.Persistence;

public static class DbInitialization
{
    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ScoutPlatformDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
