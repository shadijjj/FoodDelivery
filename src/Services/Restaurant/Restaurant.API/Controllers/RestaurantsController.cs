using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Commands;
using Restaurant.Application.DTOs;
using Restaurant.Application.Queries;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly ISender _mediator;

    public RestaurantsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<RestaurantDto>>> GetRestaurants()
    {
        var restaurants = await _mediator.Send(new GetRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateRestaurant([FromBody] CreateRestaurantCommand command)
    {
        var restaurantId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetRestaurants), new { id = restaurantId }, restaurantId);
    }

    [HttpPost("{id:guid}/menu-items")]
    public async Task<ActionResult<Guid>> AddMenuItem(Guid id, [FromBody] AddMenuItemRequest request)
    {
        var command = new AddMenuItemCommand(id, request.Name, request.Description, request.Price);
        var menuItemId = await _mediator.Send(command);
        return Ok(menuItemId);
    }
}

// Simple request wrapper for adding menu items
public record AddMenuItemRequest(string Name, string Description, decimal Price);