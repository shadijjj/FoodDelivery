namespace Restaurant.Application.DTOs;

public record MenuItemDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    bool IsAvailable
);

public record RestaurantDto(
    Guid Id,
    string Name,
    string Address,
    bool IsActive,
    List<MenuItemDto> MenuItems
);