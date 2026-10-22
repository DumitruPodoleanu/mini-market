namespace MiniMarket.Shared;

public sealed class DomainException(string message) : Exception(message);