using MediatR;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Interfaces;

namespace Restaurant.Application.Commands;

public record AddMenuItemCommand(
    Guid RestaurantId,
    string Name,
    string Description,
    decimal Price
) : IRequest<Guid>;

public class AddMenuItemCommandHandler : IRequestHandler<AddMenuItemCommand, Guid>
{
    private readonly IRestaurantDbContext _context;

    public AddMenuItemCommandHandler(IRestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(AddMenuItemCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _context.Restaurants
            .Include(r => r.MenuItems)
            .FirstOrDefaultAsync(r => r.Id == request.RestaurantId, cancellationToken);

        if (restaurant == null)
        {
            throw new KeyNotFoundException($"Restaurant with ID {request.RestaurantId} was not found.");
        }

        restaurant.AddMenuItem(request.Name, request.Description, request.Price);
        await _context.SaveChangesAsync(cancellationToken);

        // Return the ID of the newly added menu item
        return restaurant.MenuItems.Last().Id;
    }
}