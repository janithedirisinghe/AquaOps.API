using AquaOps.Domain.Common;

namespace AquaOps.Domain.Entities.Core;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Subdomain { get; set; }
    public string Country { get; set; } = "PH";
    public string Currency { get; set; } = "PHP";
    public string Timezone { get; set; } = "Asia/Manila";
    public bool IsActive { get; set; } = true;
    public DateTime? SubscriptionExpiry { get; set; }

    // Navigation
    public ICollection<Site> Sites { get; set; } = new List<Site>();
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}