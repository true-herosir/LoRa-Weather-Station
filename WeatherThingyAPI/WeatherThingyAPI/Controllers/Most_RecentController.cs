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
            query = query.Where(n => n.Time >= start_time.Value);

        if (end_time.HasValue && !start_time.HasValue)
            query = query.Where(n => n.Time <= end_time.Value);

        if (start_time.HasValue && end_time.HasValue)
            query = query.Where(n => n.Time >= start_time.Value && n.Time <= end_time.Value);

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

