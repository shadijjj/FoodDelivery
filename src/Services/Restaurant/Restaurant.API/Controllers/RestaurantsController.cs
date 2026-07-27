using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Commands;

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

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateRestaurant([FromBody] CreateRestaurantCommand command)
    {
        var restaurantId = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateRestaurant), new { id = restaurantId }, restaurantId);
    }
}