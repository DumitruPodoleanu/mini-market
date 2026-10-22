using MiniMarket.Shared;
using System.Text.RegularExpressions;

namespace MiniMarket.Catalog.Domain;

internal sealed class Brand
{
    private const int MaxNameLength = 100;

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Slug { get; private set; }

    public Brand(Guid id, string name, string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Brand name cannot be empty.");
        if (name.Trim().Length > MaxNameLength)
            throw new DomainException($"Brand name cannot exceed {MaxNameLength} characters.");
        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException("Brand slug cannot be empty.");
        if (!Regex.IsMatch(slug, "^[a-z0-9]+(-[a-z0-9]+)*$"))
            throw new DomainException("Brand Slug must be lowercase letters, digits and dashes.");

        Id = id;
        Name = name.Trim();
        Slug = slug;
    }
}