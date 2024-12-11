using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Node_locationController : ControllerBase
{
    private readonly Node_locationContext _context;

    public Node_locationController(Node_locationContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Node_location>>> GetSensorLocations(
        [FromQuery] string? id,
        [FromQuery] string? location,
        //[FromQuery] double? minBatteryStatus,
        //[FromQuery] double? maxBatteryStatus,
        [FromQuery] int page = 1,
        [FromQuery] int page_size = 10)
    {
        if (page <= 0 || page_size <= 0)
        {
            return BadRequest("Page and page_size must be positive integers.");
        }

        // Build the query dynamically
        var query = _context.Node_locations.AsQueryable();

        // Apply optional filters
        if (!string.IsNullOrWhiteSpace(id))
            query = query.Where(s => s.Node_ID == id);

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(s => s.Location == location);

        //if (minBatteryStatus.HasValue && !maxBatteryStatus.HasValue)
        //    query = query.Where(s => s.Battery_status >= minBatteryStatus.Value);

        //if (maxBatteryStatus.HasValue && !minBatteryStatus.HasValue)
        //    query = query.Where(s => s.Battery_status <= maxBatteryStatus.Value);

        //if (minBatteryStatus.HasValue && maxBatteryStatus.HasValue)
        //    query = query.Where(s => s.Battery_status >= minBatteryStatus.Value && s.Battery_status <= maxBatteryStatus.Value);

        // Calculate total items and pages
        var total_items = await query.CountAsync();
        if (total_items == 0)
            return NotFound("No records found matching the given criteria.");

        var total_pages = (int)Math.Ceiling(total_items / (double)page_size);

        if (page > total_pages)
            return NotFound($"Page {page} does not exist. Total pages: {total_pages}.");

        // Fetch paginated data
        var data = await query
            .Skip((page - 1) * page_size)
            .Take(page_size)
            .Select(n => new
            {
                node_id = n.Node_ID,
                location = n.Location,
                battery_status = n.Battery_status
            })
            .ToListAsync();

        // Return data with pagination metadata
        return Ok(new
        {
            total_items,
            total_pages,
            current_page = page,
            page_size,
            data
        });
    }


}

