using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain;

namespace Ordering.Infrastructure.Persistence;

public class OrderingDbContext : DbContext, IOrderingDbContext
{
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.CustomerPhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(o => o.DeliveryAddress).IsRequired().HasMaxLength(300);
            entity.Property(o => o.TotalAmount).HasPrecision(18, 2);

            // Access backing field for OrderItems list
            entity.Navigation(o => o.Items)
                  .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ProductName).IsRequired().HasMaxLength(150);
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);
        });
    }
}