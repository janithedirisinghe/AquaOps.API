using AquaOps.Domain.Common;
using AquaOps.Domain.Enums;

namespace AquaOps.Domain.Entities.Production;

public class ProductionBatch : TenantEntity
{
    public Guid ProductionOrderId { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public BatchStatus Status { get; set; } = BatchStatus.Pending;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid? SupervisorId { get; set; }
    public int? WorkerCount { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public ProductionOrder ProductionOrder { get; set; } = null!;
    public ICollection<BatchInput> Inputs { get; set; } = new List<BatchInput>();
    public ICollection<BatchOutput> Outputs { get; set; } = new List<BatchOutput>();
    public ICollection<QualityInspection> QualityInspections { get; set; } = new List<QualityInspection>();
}
