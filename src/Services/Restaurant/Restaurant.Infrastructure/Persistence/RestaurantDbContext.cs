using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Interfaces;
using Restaurant.Domain;

namespace Restaurant.Infrastructure.Persistence;

public class RestaurantDbContext : DbContext, IRestaurantDbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Restaurant> Restaurants => Set<Domain.Restaurant>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Restaurant entity
        modelBuilder.Entity<Domain.Restaurant>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            entity.Property(r => r.Address).IsRequired().HasMaxLength(200);

            // Access backing field for MenuItems
            entity.Navigation(r => r.MenuItems)
                  .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        // Configure MenuItem entity
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Price).HasPrecision(18, 2);
        });
    }
}