using Delivery.Application.Interfaces;
using Delivery.Domain;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Persistence;

public class DeliveryDbContext : DbContext, IDeliveryDbContext
{
    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options)
    {
    }

    public DbSet<DeliveryRecord> Deliveries => Set<DeliveryRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DeliveryRecord>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.CustomerAddress).IsRequired().HasMaxLength(300);
            entity.Property(d => d.DriverName).HasMaxLength(100);
        });
    }
}