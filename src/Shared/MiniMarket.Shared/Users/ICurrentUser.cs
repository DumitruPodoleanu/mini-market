namespace MiniMarket.Shared.Users;

public interface ICurrentUser
{
    Guid Id { get; }
}