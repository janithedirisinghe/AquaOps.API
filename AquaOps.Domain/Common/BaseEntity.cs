namespace AquaOps.Domain.Common;

/// <summary>
/// Standard base for most entities — GUID primary key + audit fields.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}

/// <summary>
/// Adds tenant isolation — every row belongs to exactly one tenant.
/// </summary>
public abstract class TenantEntity : BaseEntity
{
    public Guid TenantId { get; set; }
}

/// <summary>
/// Master-data entities that support soft-delete via IsActive flag.
/// </summary>
public abstract class MasterDataEntity : TenantEntity
{
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// High-volume append-only entities (audit_log, sync_log, inventory_transaction, temperature_reading).
/// Uses BIGINT IDENTITY instead of GUID for performance.
/// </summary>
public abstract class LedgerEntity
{
    public long Id { get; set; }
    public Guid TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
}