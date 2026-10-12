namespace AquaOps.Domain.Enums;

public enum SalesOrderStatus
{
    Draft,
    Confirmed,
    Processing,
    ReadyToShip,
    Shipped,
    Delivered,
    Cancelled,
    Returned
}

public enum CustomerType
{
    Retail,
    Wholesale,
    Distributor,
    Export,
    Institutional
}