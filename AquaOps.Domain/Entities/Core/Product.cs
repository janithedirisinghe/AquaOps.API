using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Core;

public class Product : MasterDataEntity
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid SpeciesId { get; set; }
    public string UnitOfMeasure { get; set; } = "kg";
    public bool IsCatchWeight { get; set; }
    public decimal? StandardWeight { get; set; }
    public decimal? DefaultPricePerUnit { get; set; }
    public int? ShelfLifeDays { get; set; }
    public int? ReorderLevel { get; set; }
    public string? StorageInstructions { get; set; }

    // Navigation
    public Species Species { get; set; } = null!;
}
