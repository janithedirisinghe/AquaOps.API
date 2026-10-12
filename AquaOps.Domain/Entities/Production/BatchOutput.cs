using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Production;

public class BatchOutput : TenantEntity
{
    public Guid ProductionBatchId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal WeightKg { get; set; }
    public string? LotNumber { get; set; }
    public Guid? QualityGradeId { get; set; }

    // Navigation
    public ProductionBatch ProductionBatch { get; set; } = null!;
    public Core.Product Product { get; set; } = null!;
    public QualityGrade? QualityGrade { get; set; }
}
