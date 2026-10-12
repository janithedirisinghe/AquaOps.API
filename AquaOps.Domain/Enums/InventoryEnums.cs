namespace AquaOps.Domain.Enums;

public enum InventoryStatus
{
    Available,
    Reserved,
    InTransit,
    OnHold,
    Quarantine,
    Expired,
    Consumed,
    Sold,
    Damaged,
    Returned
}

public enum TransactionType
{
    Receive,
    Issue,
    Transfer,
    Adjustment,
    Return,
    Scrap,
    ProductionInput,
    ProductionOutput
}

public enum AlertType
{
    LowStock,
    Overstock,
    ExpiryWarning,
    TemperatureExcursion,
    ReorderPoint,
    StockOut
}

public enum AlertSeverity
{
    Info,
    Warning,
    Critical
}