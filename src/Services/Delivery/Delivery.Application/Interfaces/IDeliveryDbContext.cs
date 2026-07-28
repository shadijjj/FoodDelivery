using Delivery.Domain;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Application.Interfaces;

public interface IDeliveryDbContext
{
    DbSet<DeliveryRecord> Deliveries { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}