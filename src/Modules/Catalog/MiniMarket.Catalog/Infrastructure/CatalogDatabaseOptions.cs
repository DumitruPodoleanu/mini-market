using System.ComponentModel.DataAnnotations;

namespace MiniMarket.Catalog.Infrastructure;

internal sealed class CatalogDatabaseOptions
{
    public const string SectionName = "CatalogDatabase";

    public int CommandTimeoutSeconds { get; set; }
    public int MaxRetryCount { get; set; }

    [Required]
    public string ConnectionString { get; set; } = string.Empty;
}