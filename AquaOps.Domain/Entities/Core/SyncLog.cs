using AquaOps.Domain.Common;
using AquaOps.Domain.Enums;

namespace AquaOps.Domain.Entities.Core;

public class SyncLog : LedgerEntity
{
    public Guid DeviceId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public SyncStatus Status { get; set; }
    public string? ConflictData { get; set; }
    public DateTime? ResolvedAt { get; set; }
}
