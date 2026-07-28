using FoodDelivery.Shared.Contracts;
using MassTransit;
using MediatR;
using Ordering.Application.Interfaces;
using Ordering.Domain;

namespace Ordering.Application.Commands;

public record OrderItemInputDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity
);

public record SubmitOrderCommand(
    Guid CustomerId,
    string CustomerPhoneNumber,
    string DeliveryAddress,
    List<OrderItemInputDto> Items
) : IRequest<Guid>;

public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, Guid>
{
    private readonly IOrderingDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public SubmitOrderCommandHandler(IOrderingDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        // 1. Create Domain Entity
        var order = new Order(request.CustomerId, request.CustomerPhoneNumber, request.DeliveryAddress);

        foreach (var item in request.Items)
        {
            order.AddItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity);
        }

        // 2. Save to Database (OrderingDb)
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 3. Publish Integration Event to RabbitMQ via MassTransit
        var integrationEvent = new OrderSubmittedEvent(
            order.Id,
            order.CustomerId,
            order.CustomerPhoneNumber,
            order.DeliveryAddress,
            order.TotalAmount,
            order.Items.Select(i => new OrderItemDto(
                i.ProductId,
                i.ProductName,
                i.UnitPrice,
                i.Quantity
            )).ToList(),
            order.CreatedAt
        );

        await _publishEndpoint.Publish(integrationEvent, cancellationToken);

        return order.Id;
    }
}