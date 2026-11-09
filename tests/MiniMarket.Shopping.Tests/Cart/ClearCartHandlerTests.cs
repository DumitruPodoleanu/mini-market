using Microsoft.Extensions.Logging.Abstractions;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Features.Cart.ClearCart;
using MiniMarket.Shopping.Tests.Fakes;

namespace MiniMarket.Shopping.Tests.Cart;

public sealed class ClearCartHandlerTests
{
    private readonly FakeCartRepository _repository = new();
    private readonly FakeCurrentUser _user = new();

    private ClearCartHandler Handler() => new(_repository, _user, NullLogger<ClearCartHandler>.Instance);

    [Fact]
    public async Task HandleAsync_RemovesOnlyItemsOfCurrentUser()
    {
        var otherUserId = Guid.NewGuid();
        _repository.Items.Add(new CartItemEntity { Id = Guid.CreateVersion7(), UserId = _user.Id, ProductId = Guid.NewGuid(), Quantity = 1 });
        _repository.Items.Add(new CartItemEntity { Id = Guid.CreateVersion7(), UserId = otherUserId, ProductId = Guid.NewGuid(), Quantity = 1 });

        var result = await Handler().HandleAsync(CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(otherUserId, Assert.Single(_repository.Items).UserId);
    }

    [Fact]
    public async Task HandleAsync_Succeeds_WhenCartIsAlreadyEmpty()
    {
        var result = await Handler().HandleAsync(CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
