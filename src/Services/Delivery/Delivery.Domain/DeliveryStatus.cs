namespace Delivery.Domain;

public enum DeliveryStatus
{
    Pending = 0,
    Assigned = 1,
    PickedUp = 2,
    Delivered = 3,
    Failed = 4
}