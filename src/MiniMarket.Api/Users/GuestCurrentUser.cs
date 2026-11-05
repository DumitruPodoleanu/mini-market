using MiniMarket.Shared.Users;

namespace MiniMarket.Api.Users;

internal sealed class GuestCurrentUser : ICurrentUser
{
    public Guid Id { get; } = Guid.Parse("00000000-0000-0000-0000-000000000001");
}