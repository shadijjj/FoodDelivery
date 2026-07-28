using Delivery.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveriesController : ControllerBase
{
    private readonly ISender _mediator;

    public DeliveriesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DeliveryDto>>> GetDeliveries()
    {
        var deliveries = await _mediator.Send(new GetDeliveriesQuery());
        return Ok(deliveries);
    }
}