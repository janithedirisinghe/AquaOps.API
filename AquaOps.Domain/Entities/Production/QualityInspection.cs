using AquaOps.Domain.Common;
using AquaOps.Domain.Enums;

namespace AquaOps.Domain.Entities.Production;

public class QualityInspection : TenantEntity
{
    public Guid ProductionBatchId { get; set; }
    public Guid InspectorId { get; set; }
    public InspectionResult Result { get; set; }
    public decimal? TemperatureCelsius { get; set; }
    public decimal? PhLevel { get; set; }
    public string? VisualCheck { get; set; }
    public string? SmellCheck { get; set; }
    public string? TextureCheck { get; set; }
    public string? Notes { get; set; }
    public DateTime InspectedAt { get; set; }

    // Navigation
    public ProductionBatch ProductionBatch { get; set; } = null!;
    public Core.AppUser Inspector { get; set; } = null!;
}
