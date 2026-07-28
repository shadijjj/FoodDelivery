using Microsoft.EntityFrameworkCore;
using Ordering.Domain;

namespace Ordering.Application.Interfaces;

public interface IOrderingDbContext
{
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}