using AquaOps.Domain.Common;
using AquaOps.Domain.Enums;

namespace AquaOps.Domain.Entities.Core;

public class Site : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public SiteType Type { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Tenant Tenant { get; set; } = null!;
}