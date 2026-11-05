using System.ComponentModel.DataAnnotations;

namespace MiniMarket.Infrastructure.Persistence;

public abstract class ModuleDatabaseOptions
{
    public int CommandTimeoutSeconds { get; set; }
    public int MaxRetryCount { get; set; }
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
}