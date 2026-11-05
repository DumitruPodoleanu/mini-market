using Microsoft.EntityFrameworkCore;

namespace MiniMarket.Shopping.Infrastructure.Persistence;

internal sealed class ShoppingDbContext(DbContextOptions<ShoppingDbContext> options)
    : DbContext(options), IShoppingDbContext
{
    public const string Schema = "shopping";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingDbContext).Assembly);
    }
}