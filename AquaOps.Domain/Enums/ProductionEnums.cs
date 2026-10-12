namespace AquaOps.Domain.Enums;

public enum ProductionOrderStatus
{
    Draft,
    Planned,
    Released,
    InProgress,
    Completed,
    Cancelled,
    OnHold
}

public enum BatchStatus
{
    Pending,
    InProgress,
    QualityCheck,
    Passed,
    Failed,
    Released,
    Rejected
}

public enum InspectionResult
{
    Pass,
    Fail,
    Conditional,
    Pending
}