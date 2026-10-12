using AquaOps.Domain.Enums;

namespace AquaOps.Application.DTOs.Inventory;

// ── Storage Location DTOs ──

public class StorageLocationDto
{
    public Guid Id { get; set; }
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Zone { get; set; }
    public decimal? TemperatureMin { get; set; }
    public decimal? TemperatureMax { get; set; }
    public int? CapacityKg { get; set; }
    public bool IsActive { get; set; }
}

public class CreateStorageLocationRequest
{
    public Guid SiteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Zone { get; set; }
    public decimal? TemperatureMin { get; set; }
    public decimal? TemperatureMax { get; set; }
    public int? CapacityKg { get; set; }
}

// ── Inventory Item DTOs ──

public class InventoryItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductSku { get; set; }
    public Guid StorageLocationId { get; set; }
    public string? StorageLocationName { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public string? BatchNumber { get; set; }
    public int Quantity { get; set; }
    public decimal WeightKg { get; set; }
    public InventoryStatus Status { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public decimal? UnitCost { get; set; }
}

public class ReceiveInventoryRequest
{
    public Guid ProductId { get; set; }
    public Guid StorageLocationId { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public string? BatchNumber { get; set; }
    public int Quantity { get; set; }
    public decimal WeightKg { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public Guid? SupplierId { get; set; }
    public decimal? UnitCost { get; set; }
}

public class TransferInventoryRequest
{
    public Guid InventoryItemId { get; set; }
    public Guid ToStorageLocationId { get; set; }
    public int Quantity { get; set; }
    public decimal WeightKg { get; set; }
    public string? Notes { get; set; }
}

public class AdjustInventoryRequest
{
    public Guid InventoryItemId { get; set; }
    public int QuantityChange { get; set; }
    public decimal WeightChange { get; set; }
    public string? Notes { get; set; }
}

// ── Temperature DTOs ──

public class TemperatureReadingDto
{
    public long Id { get; set; }
    public Guid StorageLocationId { get; set; }
    public string? StorageLocationName { get; set; }
    public decimal TemperatureCelsius { get; set; }
    public decimal? HumidityPercent { get; set; }
    public string? RecordedByDevice { get; set; }
    public bool IsAlert { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTemperatureReadingRequest
{
    public Guid StorageLocationId { get; set; }
    public decimal TemperatureCelsius { get; set; }
    public decimal? HumidityPercent { get; set; }
    public string? RecordedByDevice { get; set; }
}