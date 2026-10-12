using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Inventory;

public class TemperatureReading : LedgerEntity
{
    public Guid StorageLocationId { get; set; }
    public decimal TemperatureCelsius { get; set; }
    public decimal? HumidityPercent { get; set; }
    public string? RecordedByDevice { get; set; }
    public bool IsAlert { get; set; }

    // Navigation
    public StorageLocation StorageLocation { get; set; } = null!;
}
