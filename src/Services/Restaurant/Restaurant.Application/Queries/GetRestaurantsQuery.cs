using MediatR;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs;
using Restaurant.Application.Interfaces;

namespace Restaurant.Application.Queries;

public record GetRestaurantsQuery : IRequest<List<RestaurantDto>>;

public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, List<RestaurantDto>>
{
    private readonly IRestaurantDbContext _context;

    public GetRestaurantsQueryHandler(IRestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<List<RestaurantDto>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Restaurants
            .AsNoTracking() // Performance optimization for read-only queries
            .Include(r => r.MenuItems)
            .Select(r => new RestaurantDto(
                r.Id,
                r.Name,
                r.Address,
                r.IsActive,
                r.MenuItems.Select(m => new MenuItemDto(
                    m.Id,
                    m.Name,
                    m.Description,
                    m.Price,
                    m.IsAvailable
                )).ToList()
            ))
            .ToListAsync(cancellationToken);
    }
}