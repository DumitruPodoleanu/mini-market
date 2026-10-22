namespace MiniMarket.Shared;

public sealed class NotFoundException(string message) : Exception(message);