using Delivery.Application.Interfaces;
using Delivery.Domain;
using FoodDelivery.Shared.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Delivery.Application.Consumers;

public class OrderSubmittedConsumer : IConsumer<OrderSubmittedEvent>
{
    private readonly IDeliveryDbContext _context;
    private readonly ILogger<OrderSubmittedConsumer> _logger;

    public OrderSubmittedConsumer(IDeliveryDbContext context, ILogger<OrderSubmittedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("📥 Received OrderSubmittedEvent for Order ID: {OrderId}", message.OrderId);

        // 1. Create a Delivery record for this order
        var delivery = new DeliveryRecord(message.OrderId, message.DeliveryAddress);

        // 2. Automatically assign a delivery driver
        delivery.AssignDriver("Driver #42 (Speedy Sam)");

        // 3. Save record to DeliveryDb
        await _context.Deliveries.AddAsync(delivery);
        await _context.SaveChangesAsync();

        _logger.LogInformation("✅ Delivery record created successfully with ID: {DeliveryId}", delivery.Id);
    }
}