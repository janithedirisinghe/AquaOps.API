using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Production;

public class QualityGrade : MasterDataEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}
