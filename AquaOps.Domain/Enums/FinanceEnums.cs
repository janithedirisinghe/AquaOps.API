namespace AquaOps.Domain.Enums;

public enum PaymentStatus
{
    Pending,
    PartiallyPaid,
    Paid,
    Overdue,
    Refunded,
    Cancelled
}

public enum PaymentMethod
{
    Cash,
    BankTransfer,
    Cheque,
    CreditCard,
    GCash,
    Maya,
    Other
}