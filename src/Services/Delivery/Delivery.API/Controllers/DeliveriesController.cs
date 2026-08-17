using Delivery.Domain;
using Delivery.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveriesController : ControllerBase
{
    private readonly DeliveryDbContext _dbContext;

    public DeliveriesController(DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var deliveries = _dbContext.Deliveries.ToList();
        return Ok(deliveries);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
    {
        var delivery = await _dbContext.Deliveries.FindAsync(id);
        if (delivery == null) 
            return NotFound("Delivery not found");

        var cleanStatusStr = dto.Status.Replace(" ", "").ToLower();

        // Map incoming UI text directly to your existing DeliveryStatus enum
        DeliveryStatus newStatus = cleanStatusStr switch
        {
            "intransit" or "pickup" or "pickedup" => DeliveryStatus.PickedUp,
            "delivered" or "completed"           => DeliveryStatus.Delivered,
            "assigned"                          => DeliveryStatus.Assigned,
            "pending"                           => DeliveryStatus.Pending,
            "failed"                            => DeliveryStatus.Failed,
            _ => Enum.TryParse<DeliveryStatus>(dto.Status, true, out var parsed) ? parsed : (DeliveryStatus)(-1)
        };

        if ((int)newStatus == -1)
        {
            return BadRequest($"Invalid delivery status: {dto.Status}");
        }

        delivery.UpdateStatus(newStatus);
        await _dbContext.SaveChangesAsync();

        return Ok(delivery);
    }
}

public record UpdateStatusDto(string Status);