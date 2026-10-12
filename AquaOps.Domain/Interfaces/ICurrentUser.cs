namespace AquaOps.Domain.Interfaces;

public interface ICurrentUser
{
    Guid? UserId { get; }
    Guid? TenantId { get; }
    string? Email { get; }
    IEnumerable<string> Permissions { get; }
}
