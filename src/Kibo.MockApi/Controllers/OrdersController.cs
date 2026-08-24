using Microsoft.AspNetCore.Mvc;
using Kibo.MockApi.Models;
using Kibo.MockApi.Storage;

namespace Kibo.MockApi.Controllers;

[ApiController]
[Route("v1/orders")]
public class OrdersController : ControllerBase
{
    /// <summary>
    /// POST /v1/orders
    /// Creates a new order. Requires the "x-kibo-tenant" header.
    /// The order starts as "Pending" and transitions to "ReadyForFulfillment" after 5 seconds.
    /// </summary>
    [HttpPost]
    [HttpPost]
public IActionResult CreateOrder([FromBody] Order order)
{
    // Tenant header required
    if (!Request.Headers.TryGetValue("x-kibo-tenant", out var tenantHeader)
        || string.IsNullOrWhiteSpace(tenantHeader))
    {
        return Unauthorized(new { error = "Missing required header: x-kibo-tenant" });
    }

    var tenant = tenantHeader.ToString();

    // Validate tenant header
    if (!System.Text.RegularExpressions.Regex.IsMatch(
            tenant,
            @"^tenant-[A-Za-z0-9-]+$"))
    {
        return BadRequest(new { error = "Invalid x-kibo-tenant header" });
    }

    // Validate customer email
    if (string.IsNullOrWhiteSpace(order.CustomerEmail)
        || order.CustomerEmail.Length > 254)
    {
        return BadRequest(new { error = "Invalid customer email" });
    }

    // Validate line items
    if (order.LineItems == null || order.LineItems.Count == 0)
    {
        return BadRequest(new { error = "At least one line item is required" });
    }

    // Validate unit price
    if (order.LineItems.Any(item => item.UnitPrice < 0))
    {
        return BadRequest(new { error = "Unit price cannot be negative" });
    }

    // Create order
    order.Id = Guid.NewGuid();
    order.TenantId = tenant;
    order.Status = "Pending";
    
    OrderStore.Add(order);

    // Change status after 5 seconds
    _ = Task.Run(async () =>
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        OrderStore.UpdateStatus(order.Id, "ReadyForFulfillment");
    });

    return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
}

    /// <summary>
    /// GET /v1/orders/{id}
    /// Returns the current state of an order.
    /// </summary>
    [HttpGet("{id:guid}")]
    public IActionResult GetOrder(Guid id)
    {
        if (!OrderStore.TryGet(id, out var order) || order is null)
        {
            return NotFound(new { error = $"Order {id} not found" });
        }

        return Ok(order);
    }
}