using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Inventory;

public class StorageLocation : MasterDataEntity
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Zone { get; set; }
    public decimal? TemperatureMin { get; set; }
    public decimal? TemperatureMax { get; set; }
    public int? CapacityKg { get; set; }

    // Navigation
    public Core.Site Site { get; set; } = null!;
    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    public ICollection<TemperatureReading> TemperatureReadings { get; set; } = new List<TemperatureReading>();
}
