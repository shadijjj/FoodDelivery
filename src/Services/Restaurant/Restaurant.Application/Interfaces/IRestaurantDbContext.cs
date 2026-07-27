using Microsoft.EntityFrameworkCore;
using Restaurant.Domain;

namespace Restaurant.Application.Interfaces;

public interface IRestaurantDbContext
{
    DbSet<Domain.Restaurant> Restaurants { get; }
    DbSet<MenuItem> MenuItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}