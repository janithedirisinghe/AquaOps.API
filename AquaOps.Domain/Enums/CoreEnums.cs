namespace AquaOps.Domain.Enums;

public enum SiteType
{
    HeadOffice,
    Processing,
    ColdStorage,
    ReceivingStation,
    DistributionCenter
}

public enum SyncStatus
{
    Pending,
    Synced,
    Conflict,
    Failed
}

public enum AuditAction
{
    Create,
    Update,
    Delete,
    Login,
    Logout,
    Export,
    Import
}