using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Core;

public class Species : MasterDataEntity
{
    public string Code { get; set; } = string.Empty;
    public string CommonName { get; set; } = string.Empty;
    public string? ScientificName { get; set; }
    public string? LocalName { get; set; }
    public string? Category { get; set; }

    // Navigation
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
