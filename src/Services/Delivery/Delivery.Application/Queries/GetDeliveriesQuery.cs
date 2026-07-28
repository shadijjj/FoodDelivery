using Delivery.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Application.Queries;

public record DeliveryDto(
    Guid Id,
    Guid OrderId,
    string CustomerAddress,
    string DriverName,
    string Status,
    DateTime CreatedAt
);

public record GetDeliveriesQuery : IRequest<List<DeliveryDto>>;

public class GetDeliveriesQueryHandler : IRequestHandler<GetDeliveriesQuery, List<DeliveryDto>>
{
    private readonly IDeliveryDbContext _context;

    public GetDeliveriesQueryHandler(IDeliveryDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeliveryDto>> Handle(GetDeliveriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Deliveries
            .AsNoTracking()
            .Select(d => new DeliveryDto(
                d.Id,
                d.OrderId,
                d.CustomerAddress,
                d.DriverName,
                d.Status.ToString(),
                d.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}