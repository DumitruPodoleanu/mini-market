using MiniMarket.Shared.Users;

namespace MiniMarket.Shopping.Tests.Fakes;

internal sealed class FakeCurrentUser : ICurrentUser
{
    public Guid Id { get; } = Guid.NewGuid();
}
