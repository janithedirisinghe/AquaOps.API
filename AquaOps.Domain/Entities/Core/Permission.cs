using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Core;

public class Permission : BaseEntity
{
    public string Module { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
