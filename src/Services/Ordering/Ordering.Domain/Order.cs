namespace Ordering.Domain;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string CustomerPhoneNumber { get; private set; } = string.Empty;
    public string DeliveryAddress { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { } // For EF Core

    public Order(Guid customerId, string customerPhoneNumber, string deliveryAddress)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        CustomerPhoneNumber = customerPhoneNumber;
        DeliveryAddress = deliveryAddress;
        Status = OrderStatus.Submitted;
        CreatedAt = DateTime.UtcNow;
        TotalAmount = 0;
    }

    public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        var item = new OrderItem(Id, productId, productName, unitPrice, quantity);
        _items.Add(item);
        TotalAmount += unitPrice * quantity;
    }

    public void CancelOrder()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed order.");

        Status = OrderStatus.Cancelled;
    }
}