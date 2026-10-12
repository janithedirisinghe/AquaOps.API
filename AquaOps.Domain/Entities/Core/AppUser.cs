using AquaOps.Domain.Common;
using System.Data;

namespace AquaOps.Domain.Entities.Core;

public class AppUser : TenantEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid? SiteId { get; set; }
    public Guid RoleId { get; set; }
    public string PreferredLanguage { get; set; } = "en";
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public DateTime? LastLoginAt { get; set; }

    // Navigation
    public Role Role { get; set; } = null!;
    public Site? Site { get; set; }
}