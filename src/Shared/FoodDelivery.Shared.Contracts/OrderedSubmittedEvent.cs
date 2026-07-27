namespace FoodDelivery.Shared.Contracts;

public record OrderItemDto(
    Guid ProductId, 
    string ProductName, 
    decimal UnitPrice, 
    int Quantity
);

public record OrderSubmittedEvent(
    Guid OrderId,
    Guid CustomerId,
    string CustomerPhoneNumber,
    string DeliveryAddress,
    decimal TotalAmount,
    List<OrderItemDto> Items,
    DateTime SubmittedAt
);