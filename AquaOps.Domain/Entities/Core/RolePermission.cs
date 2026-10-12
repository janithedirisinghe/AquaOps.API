using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Core;

public class RolePermission : TenantEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }

    // Navigation
    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}
