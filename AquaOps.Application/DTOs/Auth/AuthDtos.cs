using System.ComponentModel.DataAnnotations;

namespace AquaOps.Application.DTOs.Auth;

public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public Guid? SiteId { get; set; }
    public Guid RoleId { get; set; }
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public IEnumerable<string> Permissions { get; set; } = Enumerable.Empty<string>();
}

public class RefreshTokenRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
    [Required, MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
}

public class TenantRegistrationRequest
{
    [Required]
    public string TenantName { get; set; } = string.Empty;
    public string Country { get; set; } = "PH";
    public string Currency { get; set; } = "PHP";
    [Required, EmailAddress]
    public string AdminEmail { get; set; } = string.Empty;
    [Required, MinLength(8)]
    public string AdminPassword { get; set; } = string.Empty;
    [Required]
    public string AdminFirstName { get; set; } = string.Empty;
    [Required]
    public string AdminLastName { get; set; } = string.Empty;
}
