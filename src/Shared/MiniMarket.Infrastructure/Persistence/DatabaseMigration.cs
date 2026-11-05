using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MiniMarket.Infrastructure.Persistence;

public static class DatabaseMigration
{
    public static async Task MigrateDatabaseAsync<TContext>(this IServiceProvider services) where TContext : DbContext
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await dbContext.Database.MigrateAsync();
    }
}