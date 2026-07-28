namespace Delivery.Domain;

public class DeliveryRecord
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string CustomerAddress { get; private set; } = string.Empty;
    public string DriverName { get; private set; } = string.Empty;
    public DeliveryStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? AssignedAt { get; private set; }

    private DeliveryRecord() { } // For EF Core

    public DeliveryRecord(Guid orderId, string customerAddress)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        CustomerAddress = customerAddress;
        Status = DeliveryStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void AssignDriver(string driverName)
    {
        if (string.IsNullOrWhiteSpace(driverName))
            throw new ArgumentException("Driver name cannot be empty.");

        DriverName = driverName;
        Status = DeliveryStatus.Assigned;
        AssignedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(DeliveryStatus newStatus)
    {
        Status = newStatus;
    }
}