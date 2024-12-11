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
       [FromQuery] string? id,
       [FromQuery] string? location,
       [FromQuery] DateTime? start_time,
       [FromQuery] DateTime? end_time,
       [FromQuery] int page = 1,
       [FromQuery] int page_size = 10)
    {
        if (page <= 0 || page_size <= 0)
        {
            return BadRequest("Page and page_size must be positive integers.");
        }

        // Build the query dynamically
        var query = _context.Most_Recents.AsQueryable();

        // Apply optional filters
        if (!string.IsNullOrWhiteSpace(id))
            query = query.Where(n => n.Node_ID == id);

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(n => n.Location == location);

        if (start_time.HasValue && !end_time.HasValue)
        {
            // Include records for the entire day of start_time
            DateTime startOfDay = start_time.Value.Date;
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            query = query.Where(n => n.Time >= startOfDay && n.Time <= endOfDay);
        }

        if (end_time.HasValue && !start_time.HasValue)
        {
            // Include records for the entire day of end_time
            DateTime startOfDay = end_time.Value.Date;
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            query = query.Where(n => n.Time >= startOfDay && n.Time <= endOfDay);
        }

        if (start_time.HasValue && end_time.HasValue)
        {
            // Use the exact provided start_time and end_time
            query = query.Where(n => n.Time >= start_time.Value && n.Time <= end_time.Value);
        }

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
                time = n.Time,
                pressure = n.Pressure,
                illumination = n.Illumination,
                humidity = n.Humidity,
                location = n.Location,
                temperature_indoor = n.Temperature_indoor,
                temperature_outdoor = n.Temperature_outdoor
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

