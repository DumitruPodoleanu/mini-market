using Microsoft.EntityFrameworkCore;

namespace MiniMarket.Infrastructure.Persistence;

public static class DbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UseModuleDatabase(
        this DbContextOptionsBuilder options,
        ModuleDatabaseOptions databaseOptions,
        string schema) =>
        options.UseNpgsql(databaseOptions.ConnectionString, npgsql =>
        {
            npgsql.MigrationsHistoryTable("__EFMigrationsHistory", schema);
            npgsql.CommandTimeout(databaseOptions.CommandTimeoutSeconds);
            npgsql.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
        });
}