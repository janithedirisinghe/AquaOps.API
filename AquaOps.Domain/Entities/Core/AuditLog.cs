using AquaOps.Domain.Common;
using AquaOps.Domain.Enums;

namespace AquaOps.Domain.Entities.Core;

public class AuditLog : LedgerEntity
{
    public Guid? UserId { get; set; }
    public AuditAction Action { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? IpAddress { get; set; }
}
