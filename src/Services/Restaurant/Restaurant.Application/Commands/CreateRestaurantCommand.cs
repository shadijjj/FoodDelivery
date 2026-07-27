using MediatR;
using Restaurant.Application.Interfaces;
using Restaurant.Domain;

namespace Restaurant.Application.Commands;

public record CreateRestaurantCommand(
    string Name, 
    string Address
) : IRequest<Guid>;

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Guid>
{
    private readonly IRestaurantDbContext _context;

    public CreateRestaurantCommandHandler(IRestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = new Domain.Restaurant(request.Name, request.Address);
        
        await _context.Restaurants.AddAsync(restaurant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return restaurant.Id;
    }
}