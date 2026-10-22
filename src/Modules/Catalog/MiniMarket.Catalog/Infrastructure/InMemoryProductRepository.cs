using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Infrastructure;

internal sealed class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products =
    [
        new Product(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Telefon Samsung Galaxy A55", 6999m, 10),
        new Product(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Cafetiera Philips", 2499.90m, 0),
        new Product(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Casti Sony WH-1000XM5", 5499m, 25),
    ];

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