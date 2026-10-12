using AquaOps.Domain.Common;
using AquaOps.Domain.Enums;

namespace AquaOps.Domain.Entities.Production;

public class ProductionOrder : TenantEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public Guid SiteId { get; set; }
    public Guid OutputProductId { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal? ActualQuantity { get; set; }
    public string? UnitOfMeasure { get; set; }
    public ProductionOrderStatus Status { get; set; } = ProductionOrderStatus.Draft;
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Core.Site Site { get; set; } = null!;
    public Core.Product OutputProduct { get; set; } = null!;
    public ICollection<ProductionBatch> Batches { get; set; } = new List<ProductionBatch>();
}
