namespace AquaOps.Domain.Enums;

public enum PurchaseOrderStatus
{
    Draft,
    Submitted,
    Approved,
    PartiallyReceived,
    Received,
    Cancelled,
    Closed
}

public enum SupplierStatus
{
    Active,
    Inactive,
    Blacklisted,
    PendingApproval
}