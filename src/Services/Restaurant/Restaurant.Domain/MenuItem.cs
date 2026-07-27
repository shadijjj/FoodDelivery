namespace Restaurant.Domain;

public class MenuItem
{
    public Guid Id { get; private set; }
    public Guid RestaurantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public bool IsAvailable { get; private set; }

    // Constructor for Entity Framework Core
    private MenuItem() { }

    public MenuItem(Guid restaurantId, string name, string description, decimal price)
    {
        Id = Guid.NewGuid();
        RestaurantId = restaurantId;
        Name = name;
        Description = description;
        Price = price;
        IsAvailable = true;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0) 
            throw new ArgumentException("Price must be greater than zero.");
        
        Price = newPrice;
    }
}