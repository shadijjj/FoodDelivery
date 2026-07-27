using MediatR;
using Restaurant.Domain;

namespace Restaurant.Application.Commands;

// 1. The Command Request (Implements IRequest<Guid> to signal it returns a Guid)
public record CreateRestaurantCommand(
    string Name, 
    string Address
) : IRequest<Guid>;

// 2. The Handler (Contains the actual business logic to create and save the entity)
public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Guid>
{
    // We will inject the DB Context interface here in Step 3!
    // For now, this handler demonstrates the MediatR flow.
    public Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        // Instantiates the domain entity using its constructor
        var restaurant = new Domain.Restaurant(request.Name, request.Address);
        
        // Returns the newly generated Guid
        return Task.FromResult(restaurant.Id);
    }
}