using AquaOps.Domain.Common;
using AquaOps.Domain.Enums;

namespace AquaOps.Domain.Entities.Inventory;

public class InventoryTransaction : LedgerEntity
{
    public Guid InventoryItemId { get; set; }
    public TransactionType Type { get; set; }
    public int Quantity { get; set; }
    public decimal WeightKg { get; set; }
    public string? ReferenceType { get; set; }
    public string? ReferenceId { get; set; }
    public string? Notes { get; set; }
    public Guid? FromLocationId { get; set; }
    public Guid? ToLocationId { get; set; }

    // Navigation
    public InventoryItem InventoryItem { get; set; } = null!;
    public StorageLocation? FromLocation { get; set; }
    public StorageLocation? ToLocation { get; set; }
}
