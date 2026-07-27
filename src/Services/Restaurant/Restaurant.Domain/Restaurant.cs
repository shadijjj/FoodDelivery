namespace Restaurant.Domain;

public class Restaurant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    
    // Navigation property for 1-to-Many relationship
    private readonly List<MenuItem> _menuItems = new();
    public IReadOnlyCollection<MenuItem> MenuItems => _menuItems.AsReadOnly();

    private Restaurant() { }

    public Restaurant(string name, string address)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        IsActive = true;
    }

    public void AddMenuItem(string name, string description, decimal price)
    {
        var menuItem = new MenuItem(Id, name, description, price);
        _menuItems.Add(menuItem);
    }
}