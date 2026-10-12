using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Production;

public class BatchInput : TenantEntity
{
    public Guid ProductionBatchId { get; set; }
    public Guid InventoryItemId { get; set; }
    public int Quantity { get; set; }
    public decimal WeightKg { get; set; }

    // Navigation
    public ProductionBatch ProductionBatch { get; set; } = null!;
    public Inventory.InventoryItem InventoryItem { get; set; } = null!;
}
