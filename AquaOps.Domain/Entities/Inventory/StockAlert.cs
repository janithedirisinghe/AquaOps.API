using AquaOps.Domain.Common;
using AquaOps.Domain.Enums;

namespace AquaOps.Domain.Entities.Inventory;

public class StockAlert : TenantEntity
{
    public AlertType Type { get; set; }
    public AlertSeverity Severity { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? ProductId { get; set; }
    public Guid? StorageLocationId { get; set; }
    public bool IsAcknowledged { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public Guid? AcknowledgedBy { get; set; }

    // Navigation
    public Core.Product? Product { get; set; }
    public StorageLocation? StorageLocation { get; set; }
}
