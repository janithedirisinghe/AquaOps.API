using AquaOps.Domain.Common;
using AquaOps.Domain.Enums;

namespace AquaOps.Domain.Entities.Inventory;

public class InventoryItem : TenantEntity
{
    public Guid ProductId { get; set; }
    public Guid StorageLocationId { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public string? BatchNumber { get; set; }
    public int Quantity { get; set; }
    public decimal WeightKg { get; set; }
    public InventoryStatus Status { get; set; } = InventoryStatus.Available;
    public DateTime? ReceivedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public Guid? SupplierId { get; set; }
    public decimal? UnitCost { get; set; }
    public string? ReferenceType { get; set; }
    public string? ReferenceId { get; set; }

    // Navigation
    public Core.Product Product { get; set; } = null!;
    public StorageLocation StorageLocation { get; set; } = null!;
    public Core.Site? Site { get; set; }
    public ICollection<InventoryTransaction> Transactions { get; set; } = new List<InventoryTransaction>();
}
