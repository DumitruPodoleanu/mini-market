using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Infrastructure;

internal sealed class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];

    public Task<Product?> GetByIdAsync(Guid id) =>
        Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<IReadOnlyList<Product>> GetAllAsync() =>
        Task.FromResult<IReadOnlyList<Product>>(_products.ToList());

    public Task AddAsync(Product product)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }
}