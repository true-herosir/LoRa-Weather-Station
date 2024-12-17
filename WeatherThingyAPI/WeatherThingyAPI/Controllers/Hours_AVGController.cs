using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherThingyAPI.Models;

namespace WeatherThingyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Hours_AVGController : ControllerBase
{
    private readonly Hours_AVGContext _context;

    public Hours_AVGController(Hours_AVGContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Hours_AVG>>> GetHoursAVG(
        [FromQuery] string? id,
        [FromQuery] string? location,
        [FromQuery] DateOnly? start_date,
        [FromQuery] DateOnly? end_date,
        [FromQuery] byte? start_hour,
        [FromQuery] byte? end_hour,
        [FromQuery] int page = 1,
        [FromQuery] int page_size = 10)
    {
        if (page <= 0 || page_size <= 0)
        {
            return BadRequest("Page and page_size must be positive integers.");
        }

        // Start building the query dynamically
        var query = _context.Hours_AVGs.AsQueryable();

        // Apply optional filters
        if (!string.IsNullOrEmpty(id))
        {
            query = query.Where(h => h.Node_ID == id);
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(h => h.Location == location);
        }

        // Filter for a specific day
        if (start_date.HasValue && !end_date.HasValue)
        {
            query = query.Where(h => h.the_day == start_date.Value);
        }

        // Filter for a date range
        if (start_date.HasValue && end_date.HasValue)
        {
            query = query.Where(h => h.the_day >= start_date.Value && h.the_day <= end_date.Value);
        }

        if (start_hour.HasValue && !end_hour.HasValue)
        {
            query = query.Where(h => h.the_hour == start_hour.Value);
        }

        if (start_hour.HasValue && end_hour.HasValue)
        {
            query = query.Where(h => h.the_hour >= start_hour.Value && h.the_hour <= end_hour.Value);
        }


        // Calculate total items and pages
        var total_items = await query.CountAsync();
        var total_pages = (int)Math.Ceiling(total_items / (double)page_size);

        if (page > total_pages && total_items > 0)
        {
            return NotFound($"Page {page} does not exist. Total pages: {total_pages}.");
        }

        // Fetch paginated data
        var data = await query
            .Skip((page - 1) * page_size)
            .Take(page_size)
            .Select(n => new
            {
                node_id = n.Node_ID,
                location = n.Location,
                n.the_day,
                n.the_hour,
                pressure = n.AVG_Pressure,
                illumination = n.AVG_Illumination,
                humidity = n.AVG_Humidity,
                temperature_indoor = n.AVG_Temperature_indoor,
                temperature_outdoor = n.AVG_Temperature_outdoor
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

