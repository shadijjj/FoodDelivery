namespace Ordering.Domain;

public enum OrderStatus
{
    Pending = 0,
    Submitted = 1,
    Preparing = 2,
    OutForDelivery = 3,
    Completed = 4,
    Cancelled = 5
}