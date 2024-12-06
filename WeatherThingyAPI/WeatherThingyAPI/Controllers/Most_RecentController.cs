using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Most_RecentController : ControllerBase
{
    private readonly Most_RecentContext _context;

    public Most_RecentController(Most_RecentContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Most_Recent>>> GetMostRecents(
       [FromQuery] string? nodeId,
       [FromQuery] string? gatewayLocation,
       [FromQuery] DateTime? startTime,
       [FromQuery] DateTime? endTime,
       [FromQuery] int page = 1,
       [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize must be positive integers.");
        }

        // Build the query dynamically
        var query = _context.Most_Recents.AsQueryable();

        // Apply optional filters
        if (!string.IsNullOrWhiteSpace(nodeId))
            query = query.Where(n => n.Node_ID == nodeId);

        if (!string.IsNullOrWhiteSpace(gatewayLocation))
            query = query.Where(n => n.Gateway_Location == gatewayLocation);

        if (startTime.HasValue && !endTime.HasValue)
            query = query.Where(n => n.Time >= startTime.Value);

        if (endTime.HasValue && !startTime.HasValue)
            query = query.Where(n => n.Time <= endTime.Value);

        if (startTime.HasValue && endTime.HasValue)
            query = query.Where(n => n.Time >= startTime.Value && n.Time <= endTime.Value);

        // Calculate total items and pages
        var totalItems = await query.CountAsync();
        if (totalItems == 0)
            return NotFound("No records found matching the given criteria.");

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        if (page > totalPages)
            return NotFound($"Page {page} does not exist. Total pages: {totalPages}.");

        // Fetch paginated data
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Return data with pagination metadata
        return Ok(new
        {
            totalItems,
            totalPages,
            currentPage = page,
            pageSize,
            data
        });
    }

}

